using UnityEngine;
// Para hacer uso de Action
using System;
[System.Serializable]
public class PoolEntity : MonoBehaviour
{
    // Almacena la pool a la que pertenece esta entidad
    [HideInInspector] public string poolID;
    // Action al que se suscribirá la pool, para capturar las entidades que vuelvan a la pool
    public static Action<PoolEntity> OnReturnToPool;
    // Para desactivar los elementos visuales del objeto
    public Renderer[] renderers;
    // Para saber si el objeto está activo
    public bool active;
    /// <summary>
    /// Acciones a realizar al extrar el objeto de la pool
    /// </summary>
    public virtual void Initialize()
    {
        active = true;
        EnableRenderers(true);
    }
    /// <summary>
    /// Activa o desactiva los elementos visuales del objeto
    /// </summary>
    /// <param name="enable"></param>
    private void EnableRenderers(bool enable)
    {
        // recorremos la lista de renderers
        foreach (Renderer rend in renderers)
        {
            // activo o desactivo el renderer según el parámetro
            rend.enabled = enable;
        }
    }
    /// <summary>
    /// Localiza y almacena los renderers del entity en un array
    /// </summary>
    [ContextMenu("Find Renderers")]
    public void FindRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
    /// <summary>
    /// Acciones para desactivar un objeto
    /// </summary>
    public virtual void Deactivate()
    {
        active = false;
        EnableRenderers(false);
    }
    /// <summary>
    /// Desactiva el objeto e informa a la pool que quiere volver pasando su propia referencia
    /// </summary>
    public void ReturnPool()
    {
        Deactivate();
        OnReturnToPool?.Invoke(this);
    }
}