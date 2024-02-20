using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Enemy : PoolEntity
{
    public string targetTagName = "Player";
    public Transform target;
    public NavMeshAgent nav;
    public Animator animator;
    public float attackDistance = 10f;
    public string enemyProjectile = "EnemyBullets";
    public Transform shootingPoint;
    public float enemyAttackTurnSpeed = 5f;

    public UnityEvent OnInitialize;
    public UnityEvent OnDeactivate;

    private void Start()
    {
        CheckForTarget(targetTagName);
    }
    private void OnEnable()
    {
        PlayerHealth.OnPlayerDead += PlayerIsDead;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDead -= PlayerIsDead;
    }
    /// <summary>
    /// Busca el transform del objetivo con el tag indicado.
    /// </summary>
    /// <param name="tag"></param>
    public void CheckForTarget(string tag)
    {
        // recuperamos todos los objetos de la escena con el tag indicado
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(tag);
        // recorremos todos los objetivos posibles
        foreach (GameObject possibleTarget in possibleTargets)
        {
            // si no hay ninguno previamente seleccionado
            if (target == null) target = possibleTarget.transform;
            // comprobamos si el targert previo se encuentra a una distancia mayor que el posible target actual
            else if (Vector3.Distance(gameObject.transform.position, target.position) > 
                        Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position))
            {
                target = possibleTarget.transform;
            }
        }
    }
    /// <summary>
    /// Método para reaccionar a la muerte del jugador, pasando la máquina de estados a Idle
    /// </summary>
    public void PlayerIsDead()
    {
        animator.Play("Idle");
    }
    public override void Initialize()
    {
        base.Initialize();
        // para asegurar que se posiciona corretamente en el navmesh al sacarlo de la pool
        nav.Warp(transform.position);
        OnInitialize?.Invoke();
    }
    public override void Deactivate()
    {
        base.Deactivate();
        OnDeactivate?.Invoke();
    }
}