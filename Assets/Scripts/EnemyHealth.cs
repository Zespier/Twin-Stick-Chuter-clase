using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable<float> {

    public float startHealth = 20;
    public float currentHealth;
    public Animator animator;
    public static Action OnDeath;
    public UnityEvent OnDeathEvent;

    private void Start() {
        Revive();
    }

    public bool IsDead() => currentHealth <= 0;

    public void TakeDamage(float amount, Vector3 impactPosition = default) {
        if (IsDead()) { return; }

        currentHealth -= amount;
        if (IsDead()) {
            Death();
        }
    }

    private void Death() {
        OnDeathEvent?.Invoke();
        animator.SetTrigger("Dead");
        OnDeath?.Invoke();
    }

    public void Revive() {
        animator.SetTrigger("Idle");
        currentHealth = startHealth;
    }
}
