using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileFX : MonoBehaviour {

    public UnityEvent<Vector3> OnImpact;
    public UnityEvent OnInitialize;
    public Projectile projectile;

    private void OnEnable() {
        projectile.OnImpact += OnImpact.Invoke;
        projectile.OnInitialize += OnInitialize.Invoke;
    }

    private void OnDestroy() {
        projectile.OnImpact -= OnImpact.Invoke;
        projectile.OnInitialize -= OnInitialize.Invoke;
    }

}
