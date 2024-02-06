using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool {

    public string id;
    public PoolEntity prefab;
    public int prewarm;
    public Queue<PoolEntity> pool = new Queue<PoolEntity>();

}
