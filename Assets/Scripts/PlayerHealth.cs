using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
public class PlayerHealth : MonoBehaviour, IDamageable<float> {
    public float maxHealth = 100;
    public float currentHealth;
    [Header("HUD")]
    public Image healthBar;
    public Image damageImage;
    public float flashSpeed = 5;
    public Color flashColor = new Color(1, 1, 1, 0.5f);
    [Header("OnDead")]
    public UnityEvent OnDead;
    public static Action OnPlayerDead;
    public PlayerController playerController;
    public bool damaged;
    public bool isDead;
    private void Start() {
        currentHealth = maxHealth;
    }
    private void Update() {
        FlashDamage();
    }
    public bool IsDead() {
        return currentHealth <= 0;
    }
    [ContextMenu("Test take damage")]
    public void TestTakeDamage() => TakeDamage(30);
    public void TakeDamage(float damage, Vector3 impactPosition = default) {
        if (isDead) return;
        damaged = true;
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (IsDead()) Death();
    }
    private void Death() {
        isDead = true;
        OnPlayerDead?.Invoke();
        OnDead.Invoke();
        playerController.enabled = false;
    }
    private void FlashDamage() {
        if (damaged) {
            damageImage.color = flashColor;
            damaged = true;
        } else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }
}