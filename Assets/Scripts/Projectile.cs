using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolEntity {

    [Header("Components")]
    public Collider collider;
    public Rigidbody rb;
    public ParticleSystem trail;

    [Header("Projectile")]
    public float damage = 10;
    public float speed = 10f;
    public float lifeTime = 5;
    public LayerMask shootableLayer;

    private float lifeTimeStamp;

    private void Update() {
        if (lifeTimeStamp < Time.time && active) {
            ReturnPool();
        }
    }

    [ContextMenu("Get Components")]
    public void GetComponents() {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponentInChildren<ParticleSystem>();
    }

    public override void Initialize() {
        base.Initialize();

        collider.enabled = true;
        rb.isKinematic = false;
        trail.Play();

        rb.velocity = transform.forward * speed;
        lifeTimeStamp = Time.time + lifeTime;
    }

    public override void Deactivate() {
        base.Deactivate();

        collider.enabled = false;
        rb.isKinematic = true;
        trail.Stop();
    }

    private void OnTriggerEnter(Collider other) {
        if ((shootableLayer & (1 << other.gameObject.layer)) != 0) {

        }
    }
}
