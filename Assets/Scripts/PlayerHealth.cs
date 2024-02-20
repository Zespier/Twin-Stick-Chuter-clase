using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
public class PlayerHealth : MonoBehaviour, IDamageable<float>
{
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
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        FlashDamage();
    }
    /// <summary>
    /// Devuelve true si el jugador está muerto.
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    [ContextMenu("Test take damage")]
    public void TestTakeDamage() => TakeDamage(30);
    /// <summary>
    /// Aplica el daño recibido como parámetro.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="impactPosition"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void TakeDamage(float damage, Vector3 impactPosition = default)
    {
        if (isDead) return;
        damaged = true;
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (IsDead()) Death();
    }
    /// <summary>
    /// Método que se encargará de realizar las acciones necesarias para la muerte del jugador
    /// </summary>
    private void Death()
    {
        // indicamos que el jugador está muerto
        isDead = true;
        // verificamos si hay alguien suscrito al evento de dead, si es así, lo invocamos
        OnPlayerDead?.Invoke();
        // unity event propio para gestión de acciones y efectos
        OnDead.Invoke();
        // desactivamos el script que gestiona el movimiento del jugador
        // esto mismo lo podríamos hacer directamente en los unity events
        playerController.enabled = false;
    }
    /// <summary>
    /// Gestiona la imagen de daño mostrada al jugador
    /// </summary>
    private void FlashDamage()
    {
        // Si se ha dañado en este ciclo
        if (damaged)
        {
            // Aplicamos el color de daño
            damageImage.color = flashColor;
            // reiniciamos la variable para la siguiente comprobación de daño
            damaged = true;
        }
        else
        {
            // si no es dañado nuevamente, de forma progresiva iremos retirando la imagen de daño
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }
}