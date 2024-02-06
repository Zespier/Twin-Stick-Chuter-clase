using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolEntity : MonoBehaviour {

    public string poolID;
    public static Action<PoolEntity> OnReturnToPool;
    public Renderer[] renderers;
    public bool active;


    public virtual void Initialize() {
        active = true;
        EnableRenderers(true);
    }

    public virtual void Deactivate() {
        active = false;
        EnableRenderers(false);
    }

    public void ReturnPool() {
        Deactivate();
        OnReturnToPool?.Invoke(this);
    }

    private void EnableRenderers(bool enable) {
        foreach (Renderer renderer in renderers) {
            renderer.enabled = enable;
        }
    }

    [ContextMenu("Find Renderers")]
    public void FindRenderers() {
        renderers = GetComponentsInChildren<Renderer>();
    }
}
