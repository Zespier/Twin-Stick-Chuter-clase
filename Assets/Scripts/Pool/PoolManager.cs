using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolManager : MonoBehaviour {

    public static PoolManager instance;
    public Pool[] pools;

    private void Awake() {
        if (!instance) {
            instance = this;
        }

        InitializePools();
    }

    private void OnEnable() {
        PoolEntity.OnReturnToPool += Push;
    }

    private void OnDisable() {
        PoolEntity.OnReturnToPool -= Push;
    }

    public void Push(PoolEntity entity) {

        foreach (Pool pool in pools.Where(p => p.id == entity.poolID).AsEnumerable()) {
            pool.pool.Enqueue(entity);
        }
    }

    private PoolEntity CreatePoolEntity(string poolID) {

        PoolEntity entity = null;

        Pool pool = pools.Where(p => p.id == poolID).FirstOrDefault();

        if (pool != null) {
            entity = Instantiate(pool.prefab, transform);

            entity.poolID = pool.id;
        }

        return entity;
    }

    private void InitializePools() {
        foreach (Pool pool in pools) {
            for (int i = 0; i < pool.prewarm; i++) {
                PoolEntity temp = CreatePoolEntity(pool.id);
                temp.Deactivate();
                pool.pool.Enqueue(temp);
            }
        }
    }

    public PoolEntity Pull(string poolID, Vector3 position, Quaternion rotation) {

        PoolEntity entity = null;

        Pool pool = pools.Where(p => p.id == poolID).FirstOrDefault();
        if (pool != null) {

            if (!pool.pool.TryDequeue(out entity)) {
                entity = CreatePoolEntity(poolID);
            }
        }

        if (entity) {
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            entity.Initialize();
        }

        return entity;
    }
}
