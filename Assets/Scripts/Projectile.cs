using System;
using UnityEngine;
public class Projectile : PoolEntity
{
    [Header("Components")]
    public Collider col;
    public Rigidbody rigidBody;
    public ParticleSystem trail;
    [Header("Projectile")]
    public int damage = 10;
    public float speed = 10f;
    public float lifeTime = 5f;
    private float lifeTimeStamp;
    public LayerMask shootableLayer;

    public Action<Vector3> onImpact;
    public Action onInitialize;
    private void Update()
    {
        if (lifeTimeStamp < Time.time && active) ReturnPool();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 11111000
        // 10010010
        // --------
        // 10010000
        // Si el resultado final fuera 0 completamente => significa que el layer no está contenido en la máscara
        if ((shootableLayer & (1 << other.gameObject.layer)) != 0)
        {
            // tratamos de recuperrar el componente
            if (other.TryGetComponent(out IDamageable<float> damageable))
            {
                // Si es posible, aplicamos daño
                damageable.TakeDamage(damage, transform.position);
            }
            onImpact?.Invoke(transform.position);
            ReturnPool();
        }
    }
    [ContextMenu("GetComponents")]
    public void GetComponents()
    {
        col = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();
        trail = GetComponentInChildren<ParticleSystem>();
    }
    /// <summary>
    /// Inicializa los componentes específicos del proyectil
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        onInitialize?.Invoke();
        col.enabled = true;
        rigidBody.isKinematic = false;
        trail.Play();
        rigidBody.velocity = transform.forward * speed;
        lifeTimeStamp = Time.time + lifeTime;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        col.enabled = false;
        rigidBody.isKinematic = true;
        trail.Stop();
    }
}