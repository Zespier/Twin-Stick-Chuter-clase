using System;
using System.CodeDom.Compiler;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES
    public bool showGizmos = false;
    [Header("Player Movement")]
    public float movementSpeed = 8f;
    public float rotationSpeed = 14f;
    public float acceleration = 30f;
    private Vector3 direction = Vector3.zero;
    private Vector3 desiredVelocity = Vector3.zero;
    private float horizontal = 0f;
    private float vertical = 0f;

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashForce = 10f;

    [Header("Physics")]
    public Rigidbody rigidBody;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private bool grounded;
    public Vector3 groundCheckSize;
    private Collider[] colliderBuffer;

    [Header("Collision pre-detection")]
    public LayerMask checkLayer;
    public Transform checkPoint;
    public float checkSize = 0.3f;
    [Range(0, 3)] public float checkDistance = 2f;
    public bool walled;

    [Header("Engine Sound")]
    public AudioSource audioSource;
    public float engineBasePitch = 0.4f;
    public float engineMaxSpeed = 3f;

    [Header("Effects")]
    public ParticleSystem[] dustParticles;

    [Header("Animator")]
    public Animator animator;
    public Animator animatorL;
    public Animator animatorR;

    [Header("Shooting")]
    public float shootDelay = 0.5f;
    private float shootTime = 0;
    private bool leftCannon = true;
    public Transform cannonLeft;
    public Transform cannonRight;
    // id de la pool de la cual cogeremos la bala
    public string bulletType = "RegularBullets";

    [Header("Aiming")]
    public float camRayLenght;
    public LayerMask pointerLayer;
    public Transform aimingPivot;
    private Camera cameraMain;
    #endregion

    #region EVENTS
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(checkPoint.position, checkSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
    private void Start()
    {
        // Solo vamos a comprobar is es mayor que 0, por tanto, no necesitamos más capacidad
        colliderBuffer = new Collider[1];
        cameraMain = Camera.main;
    }
    private void Update()
    {
        GroundCheck();
        CollisionPreDetection();
        Controls();
        Movement();
        MovementFX();
        AnimatorFeed();
        AimingBehaviour();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// Recuperamos la información de los inputs.
    /// </summary>
    private void Controls()
    {
        // Recuperamos la información de los axes de control
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump")) Jump();
        if (Input.GetButtonDown("Dash")) Dash();
        if (Input.GetButton("Fire1")) Shoot();
    }
    /// <summary>
    /// Realiza el desplazamiento del tanque.
    /// </summary>
    private void Movement()
    {
        // Componemos el vector de dirección deseado a partir del input
        direction.Set(horizontal, 0f, vertical);
        // Para asegurarnos que las diagonales no tienen una magnitud superior a 1, "clampeamos" su valor
        direction = Vector3.ClampMagnitude(direction, 1f);
        // Calculamos la velocidad deseada en base a la dirección y la velocidad máxima
        desiredVelocity = direction * movementSpeed;
        Vector3 tmp = transform.position + direction * checkDistance;
        // respetamos la altura a la que ya estuviera el checkpoint configurado
        tmp.y = checkPoint.position.y;
        // Movemos el checkpoint a su posición final
        checkPoint.position = tmp;
        if (walled) desiredVelocity = Vector3.zero;
        // Solo realizaremos desplazamiento y rotación, si el input es distinto de 0
        if ((horizontal != 0 || vertical != 0) && grounded && !walled)
        {
            // Aplicamos la velocidad deseada, aumentando frame a frame en base a la aceleración
            rigidBody.velocity = Vector3.MoveTowards(rigidBody.velocity,
                                                     desiredVelocity,
                                                     Time.deltaTime * acceleration);
            // Debug ray para ver la dirección de velocidad del rigidbody
            if (showGizmos) Debug.DrawRay(transform.position, rigidBody.velocity);
            // Rotamos el tanque para que mire hacia la dirección que apunta la velocidad deseada
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(desiredVelocity),
                                                  Time.deltaTime * rotationSpeed);
        }
        // En caso de no existir input, paramos la rotación del tanque
        if ((horizontal == 0 && vertical == 0) || walled) rigidBody.angularVelocity = Vector3.zero;
    }
    /// <summary>
    /// Efectos especiales visuales y sonoros.
    /// </summary>
    private void MovementFX()
    {
        // Modificamos el pitch del motor de forma dinámica en base a la velocidad del rigidbody
        audioSource.pitch = Mathf.Clamp(engineBasePitch + rigidBody.velocity.magnitude,
                                        engineBasePitch,
                                        engineMaxSpeed);
        // Si estamos en contacto con el suelo
        if (grounded)
        {
            // recorremos todos los sistemas de partículas de polvo
            foreach (ParticleSystem ps in dustParticles)
            {
                // Si no se reproducen, lo hacemos
                if (!ps.isPlaying) ps.Play();
            }
        }
        else
        {
            foreach (ParticleSystem ps in dustParticles)
            {
                ps.Stop();
            }
        }
    }
    /// <summary>
    /// Alimenta la información para los animators.
    /// </summary>
    private void AnimatorFeed()
    {
        animator.SetBool("Is Grounded", grounded);
        animatorL.SetBool("Is Grounded", grounded);
        animatorR.SetBool("Is Grounded", grounded);

        animator.SetFloat("Forward Speed", rigidBody.velocity.magnitude);
        animatorR.SetFloat("Forward Speed", rigidBody.velocity.magnitude);
        animatorL.SetFloat("Forward Speed", rigidBody.velocity.magnitude);

        animator.SetFloat("Turn", rigidBody.angularVelocity.magnitude);

        animator.SetFloat("Velocity V", rigidBody.velocity.y);
        animatorR.SetFloat("Velocity V", rigidBody.velocity.y);
        animatorL.SetFloat("Velocity V", rigidBody.velocity.y);
    }
    /// <summary>
    /// Comprueba si hay contacto con el suelo.
    /// </summary>
    private void GroundCheck()
    {
        colliderBuffer[0] = null;
        // Comprobamos si hay contacto con el suelo mediante un overlap nonAlloc, para consumir menos recursos 
        // ya que esta comprobación se hará de forma continua
        Physics.OverlapBoxNonAlloc(groundCheck.position,
                                   groundCheckSize / 2,
                                   colliderBuffer,
                                   transform.rotation,
                                   groundLayer);
        // Si el valor del primer elemento del buffer es distinto de null, consideramos que estamos tocando el suelo
        grounded = colliderBuffer[0] != null;
    }
    /// <summary>
    /// Detecta si hay un collider del layer indicado en contacto con el checker de colisión de desplazamiento.
    /// </summary>
    private void CollisionPreDetection()
    {
        colliderBuffer[0] = null;
        Physics.OverlapSphereNonAlloc(checkPoint.position,
                                      checkSize,
                                      colliderBuffer,
                                      checkLayer);
        walled = colliderBuffer[0] != null;
    }
    /// <summary>
    /// Aplica una fuerza vertical para saltar.
    /// </summary>
    private void Jump()
    {
        // Si estamos en contacto con el suelo, aplicamos una fuerza vertical de tipo impulso
        if (grounded) rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    /// <summary>
    /// Aplica una fuerza en la dirección de movimiento.
    /// </summary>
    private void Dash()
    {
        if (grounded) rigidBody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
    }
    /// <summary>
    /// Dispara el cañón L o R
    /// </summary>
    private void Shoot()
    {
        if (Time.time < shootTime) return;
        if (leftCannon)
        {
            animator.SetTrigger("Shoot Left");
            PoolManager.instance.Pull(bulletType, cannonLeft.position, Quaternion.LookRotation(cannonLeft.forward));
        }
        else
        {
            animator.SetTrigger("Shoot Right");
            PoolManager.instance.Pull(bulletType, cannonRight.position, Quaternion.LookRotation(cannonRight.forward));
        }
        shootTime = Time.time + shootDelay;
        leftCannon = !leftCannon;
        animator.SetFloat("ShootSpeed", 1 / shootDelay);
    }
    /// <summary>
    /// Apuntado de la torreta hacia la posición del cursor en pantalla.
    /// </summary>
    private void AimingBehaviour()
    {
        Ray camRay = cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit = new RaycastHit();
        if (Physics.Raycast(camRay, out groundHit, camRayLenght, pointerLayer))
        {
            aimingPivot.position = new Vector3(groundHit.point.x, aimingPivot.position.y, groundHit.point.z);
        }
    }
    #endregion
}