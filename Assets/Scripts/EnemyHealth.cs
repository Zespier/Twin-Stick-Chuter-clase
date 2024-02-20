using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable<float> {

    public float maxHealth;
    public float currentHealth;
    public Animator animator;
    public static Action OnDead;
    public UnityEvent OnDeadEvent;

    [Header("Dissolve")]
    public Renderer renderer;
    public float dissolveTime = 5f;
    public float dissolveMaxHeight = 2f;
    public float dissolveMinHeight = -1.5f;

    private MaterialPropertyBlock _dissolverPropertyBlock;
    private float dissolveCounter;
    private float dissolveCurrentHeight;
    private Coroutine c_dissolveCoroutine;

    public UnityEvent OnDissolve;

    private void Awake() {
        _dissolverPropertyBlock = new MaterialPropertyBlock();
    }

    private void Start() {
        Revive();
    }
    /// <summary>
    /// Indica si el enemigo ha muerto
    /// </summary>
    /// <returns></returns>
    public bool IsDead() {
        return currentHealth <= 0;
    }
    /// <summary>
    /// Aplica el daño recibido como parámetro
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="impactPosition"></param>
    public void TakeDamage(float damage, Vector3 impactPosition = default) {
        if (IsDead()) return;
        currentHealth -= damage;
        if (IsDead()) Death();
    }
    /// <summary>
    /// Realiza las acciones necesarias para gestionar la muerte del enemigo
    /// </summary>
    private void Death() {
        OnDeadEvent?.Invoke();
        animator.SetTrigger("Dead");
        OnDead?.Invoke();

        if (c_dissolveCoroutine != null) {
            StopCoroutine(c_dissolveCoroutine);
        }
        c_dissolveCoroutine = StartCoroutine(C_DissolveCoroutine());
    }
    public void Revive() {
        currentHealth = maxHealth;

        if (c_dissolveCoroutine != null) {
            StopCoroutine(c_dissolveCoroutine);
        }

        animator.ResetTrigger("Dead");

        animator.Play("Forward");

        renderer.GetPropertyBlock(_dissolverPropertyBlock);
        _dissolverPropertyBlock.SetFloat("_CutoffHeight", dissolveMaxHeight);
        renderer.SetPropertyBlock(_dissolverPropertyBlock);
    }

    private IEnumerator C_DissolveCoroutine() {
        dissolveCounter = dissolveTime;

        while (dissolveCounter >= 0) {
            renderer.GetPropertyBlock(_dissolverPropertyBlock);

            _dissolverPropertyBlock.SetFloat("_CutoffHeight", Mathf.Lerp(dissolveMaxHeight, dissolveMinHeight, 1 - dissolveCounter / dissolveTime));

            renderer.SetPropertyBlock(_dissolverPropertyBlock);

            dissolveCounter -= Time.deltaTime;

            yield return null;
        }

        OnDissolve?.Invoke();

    }
}