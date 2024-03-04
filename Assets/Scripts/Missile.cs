using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Missile : PoolEntity {

    [Header("Missile")]
    public float damage = 20f;
    public float damageAreaRadius = 3f;
    public float speed = 10f;
    public float lifeTime = 5f;
    public LayerMask shootableLayer;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public Vector3 shooterPosition;

    private float lifeCounter;
    private IDamageable<float> damageable;

    public UnityEvent OnInitialize;
    public UnityEvent OnImpact;
    public UnityEvent OnDeactivate;

    public ShakeScriptable shakeProfile;

    private void Update() {
        if (lifeCounter < -1 && active) {
            ReturnPool();
        }

        transform.position = Vector3.Slerp(startPosition - shooterPosition, targetPosition - shooterPosition, 1 - lifeCounter / lifeTime) + shooterPosition;
    }

    private void OnTriggerEnter(Collider other) {

        if ((shootableLayer & (1 << other.gameObject.layer)) != 0) {

            Collider[] impacts = Physics.OverlapSphere(transform.position, damageAreaRadius, shootableLayer);

            foreach (Collider i in impacts) {

                damageable = null;

                if (i.TryGetComponent(out damageable)) {
                    damageable.TakeDamage(damage, transform.position);
                }
            }

            targetPosition = transform.position;

            CinemachineShake.instance.StartShake(shakeProfile);

            OnImpact?.Invoke();

            ReturnPool();
        }
    }

    public override void Initialize() {
        base.Initialize();
        lifeCounter = lifeTime;
        OnInitialize?.Invoke();
    }

    public override void Deactivate() {
        base.Deactivate();
        OnDeactivate?.Invoke();
    }
}