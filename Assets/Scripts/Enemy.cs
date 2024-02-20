using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : PoolEntity {
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

    private void OnEnable() {
        PlayerHealth.OnPlayerDead += PlayerIsDead;

    }

    private void Start() {
        CheckForTarget(targetTagName);
    }

    private void OnDisable() {
        PlayerHealth.OnPlayerDead -= PlayerIsDead;
    }

    public void CheckForTarget(string tag) {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject possibleTarget in possibleTargets) {
            if (target == null) target = possibleTarget.transform;
            else if (Vector3.Distance(gameObject.transform.position, target.position) >
                        Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position)) {
                target = possibleTarget.transform;
            }
        }
    }

    public void PlayerIsDead() {
        animator.Play("Idle");
    }

    public override void Initialize() {
        base.Initialize();
        nav.Warp(transform.position);
        OnInitialize.Invoke();
    }

    public override void Deactivate() {
        base.Deactivate();
        OnDeactivate.Invoke();
    }
}