using System.Collections.Generic;
[System.Serializable]
public class Pool
{
    public string id;
    public PoolEntity prefab;
    public int prewarm;
    public Queue<PoolEntity> pool = new Queue<PoolEntity>();
}