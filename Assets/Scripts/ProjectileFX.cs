using UnityEngine;
using UnityEngine.Events;
public class ProjectileFX : MonoBehaviour
{
    public UnityEvent<Vector3> onImpact;
    public UnityEvent onInitialize;
    public Projectile projectile;
    private void OnEnable()
    {
        projectile.onImpact += onImpact.Invoke;
        projectile.onInitialize += onInitialize.Invoke;
    }
    private void OnDestroy()
    {
        projectile.onImpact -= onImpact.Invoke;
        projectile.onInitialize -= onInitialize.Invoke;
    }
}