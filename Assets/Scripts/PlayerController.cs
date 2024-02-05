using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Variables

    public bool showGizmos = false;

    [Header("Player Movement")]
    public float movementSpeed = 8f;
    public float rotationSpeed = 14f;
    public float acceleration = 30f;

    private Vector3 _direction;
    private Vector3 _desiredVelocity;
    private float _horizontal = 0f;
    private float _vertical = 0f;

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashForce = 10f;

    [Header("Physics")]
    public Rigidbody rb;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector3 groundCheckSize;

    private bool _grounded;

    [Header("Collision pre-detection")]
    public LayerMask checkLayer;
    public Transform checkPoint;
    public float checkSize = 0.3f;
    [Range(0, 3)] public float checkDistance = 2;
    public bool _walled;

    [Header("Engine Sound")]
    public AudioSource audioSource;
    public float engineBasePitch = 0.4f;
    public float engineMaxPitch = 3f;

    [Header("Effects")]
    public ParticleSystem[] dustParticles;

    [Header("Animator")]
    public Animator animator;
    public Animator animatorL;
    public Animator animatorR;

    #endregion
    Collider[] colliderBuffer = new Collider[1];


    #region Events

    private void Update() {
        _grounded = true;

        GroundCheck();
        CollisionPreDetection();

        Controls();
        Movement();
        MovementFX();

        AnimatorFeed();
    }

    private void OnDrawGizmos() {
        if (!showGizmos) { return; }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(checkPoint.position, checkSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    #endregion


    #region Methods

    private void Controls() {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump")) {
            Jump();
        }

        if (Input.GetButtonDown("Dash")) {
            Dash();
        }
    }

    private void Movement() {

        _direction.Set(_horizontal, 0f, _vertical);

        _direction = Vector3.ClampMagnitude(_direction, 1f);

        _desiredVelocity = _direction * movementSpeed;

        Vector3 temp = transform.position + _direction * checkDistance;
        temp.y = checkPoint.position.y;
        checkPoint.position = temp;

        if (_walled) { _desiredVelocity = Vector3.zero; }

        if ((_horizontal != 0 || _vertical != 0) && _grounded && !_walled) {

            rb.velocity = Vector3.MoveTowards(rb.velocity, _desiredVelocity, Time.deltaTime * acceleration);

            if (showGizmos) {
                Debug.DrawRay(transform.position, rb.velocity);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_desiredVelocity), Time.deltaTime * rotationSpeed);
        }

        if (_horizontal == 0 && _vertical == 0) { rb.angularVelocity = Vector3.zero; }

    }

    private void MovementFX() {
        //audioSource.pitch = Mathf.Clamp(engineBasePitch + rb.velocity.magnitude, engineBasePitch, engineMaxPitch);
        audioSource.pitch = Mathf.Lerp(engineBasePitch, engineMaxPitch, rb.velocity.magnitude / movementSpeed);

        if (_grounded) {
            foreach (ParticleSystem ps in dustParticles) {
                if (!ps.isPlaying) {
                    ps.Play();
                }
            }
        } else {
            foreach (ParticleSystem ps in dustParticles) {
                ps.Stop();
            }
        }
    }

    private void AnimatorFeed() {
        animator.SetBool("Is Grounded", _grounded);
        animatorL.SetBool("Is Grounded", _grounded);
        animatorR.SetBool("Is Grounded", _grounded);

        animator.SetFloat("Forward Speed", rb.velocity.magnitude);
        animatorL.SetFloat("Forward Speed", rb.velocity.magnitude);
        animatorR.SetFloat("Forward Speed", rb.velocity.magnitude);

        animator.SetFloat("Turn", rb.angularVelocity.magnitude);

        animator.SetFloat("Velocity V", rb.velocity.y);
        animatorL.SetFloat("Velocity V", rb.velocity.y);
        animatorR.SetFloat("Velocity V", rb.velocity.y);

        float maxVelocity = 10;

        if (rb.velocity.magnitude > maxVelocity) {

        }

        if (rb.velocity.sqrMagnitude > maxVelocity * maxVelocity) {

        }
    }

    private void GroundCheck() {
        colliderBuffer[0] = null;

        Physics.OverlapBoxNonAlloc(groundCheck.position, groundCheckSize / 2f, colliderBuffer, transform.rotation, groundLayer);

        _grounded = colliderBuffer[0] != null;
    }

    private void CollisionPreDetection() {
        colliderBuffer[0] = null;

        Physics.OverlapSphereNonAlloc(checkPoint.position, checkSize, colliderBuffer, checkLayer);

        _walled = colliderBuffer[0] != null;
    }

    private void Jump() {
        if (_grounded) { rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }

    }

    private void Dash() {
        if (_grounded) { rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse); }
    }

    #endregion

}
