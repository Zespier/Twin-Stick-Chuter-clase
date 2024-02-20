using UnityEngine;
using System.Linq;
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public Pool[] pools;
    private void Awake()
    {
        if (!instance) instance = this;
    }
    private void Start()
    {
        InitializePools();
    }
    private void OnEnable()
    {
        PoolEntity.OnReturnToPool += Push;
    }
    private void OnDisable()
    {
        PoolEntity.OnReturnToPool -= Push;
    }
    /// <summary>
    /// Vuelve a meter un entity en su pool correspondiente.
    /// Este método será el que utilizaremos para suscribirnos al Action de los PoolEntity.
    /// </summary>
    /// <param name="entity"></param>
    public void Push(PoolEntity entity)
    {
        // Intento recuperar el pool que cumple con la condición de tner el mismo ID que el entity recibido como parámetro
        Pool pool = pools.Where(s => s.id == entity.poolID).FirstOrDefault();
        // si el resultado no es null, lo agrego a la  pool de vuelta
        //if (pool != null) pool.pool.Enqueue(entity);
        pool?.pool.Enqueue(entity);

        // Busco la pool cuyo ID coincide con el ID del entity recibido como parámetro.
        //foreach (Pool pool in pools.Where(s => s.id == entity.poolID).AsEnumerable())
        //{
        //    // Si encuentra la pool, agrega el entity de vuelta a su pool
        //    pool.pool.Enqueue(entity);
        //}
    }
    /// <summary>
    /// Crea un nuevo PoolEntity del pool indicado.
    /// </summary>
    /// <param name="poolID"></param>
    /// <returns></returns>
    private PoolEntity CreatePoolEntity(string poolID)
    {
        // Variable para almacenar el nuevo entity generado
        PoolEntity entity = null;
        // Buscamos la pool con el ID indicado como parámetro
        Pool pool = pools.Where(s => s.id == poolID).FirstOrDefault();
        // Si encontramos la pool
        if (pool != null)
        {
            // Instanciamos el entity con el prefab de la pool
            entity = Instantiate(pool.prefab, transform);
            // Asignamos al nuevo entity el ID de la pool
            entity.poolID = pool.id;
        }
        // Devolvemos el nuevo entity generado
        return entity;
    }
    /// <summary>
    /// Inicializa todas las pools
    /// </summary>
    private void InitializePools()
    {
        // Recorremos todas las pools
        foreach (Pool pool in pools)
        {
            // Repetimos esta operación tantas veces como prewarm se haya especificado
            for (int i = 0; i < pool.prewarm; i++)
            {
                // Instanciamos una nueva entidad
                PoolEntity temp = CreatePoolEntity(pool.id);
                // La dejamos desactivada
                temp.Deactivate();
                // La ponemos en cola
                pool.pool.Enqueue(temp);
            }
        }
    }
    /// <summary>
    /// Extrae un entity de la pool indicada y lo posiciona, rota y activa con los parámetros indicados
    /// </summary>
    /// <param name="poolID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public PoolEntity Pull(string poolID, Vector3 position, Quaternion rotation)
    {
        // Variable para contener el entity resultante
        PoolEntity entity = null;
        // buscamos el pool que tenga el ID indicado
        Pool pool = pools.Where(s => s.id == poolID).FirstOrDefault();
        // si existe la pool
        if (pool != null)
        {
            // si no ha sido posible realizar un Dequeue (si no quedan elementos en la pool)
            if (!pool.pool.TryDequeue(out entity))
            {
                // creamos un nuevo entity en su lugar y lo entregamos
                entity = CreatePoolEntity(pool.id);
            }
        }
        if(entity != null)
        {
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            entity.Initialize();
        }
        return entity;
    }
}