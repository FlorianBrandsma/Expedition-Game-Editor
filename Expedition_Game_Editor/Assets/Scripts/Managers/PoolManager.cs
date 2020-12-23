using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    //e.g. all models
    public class Pool
    {
        public Enums.ElementType elementType;
        public List<ObjectPool> objectPoolList = new List<ObjectPool>();

        public Pool(Enums.ElementType elementType)
        {
            this.elementType = elementType;
        }

        public void Add(IPoolable poolObject)
        {
            if (!objectPoolList.Any(x => x.poolId == poolObject.PoolId))
                objectPoolList.Add(new ObjectPool(poolObject.PoolId));

            objectPoolList.Where(x => x.poolId == poolObject.PoolId).FirstOrDefault().pool.Add(poolObject);
        }
    }

    //e.g. all models with id 1
    public class ObjectPool
    {
        public int poolId;
        public List<IPoolable> pool = new List<IPoolable>();

        public ObjectPool(int poolId)
        {
            this.poolId = poolId;
        }
    }

    static public PoolManager instance;

    static public List<Pool> poolList = new List<Pool>();

    private void Awake()
    {
        instance = this;
    }

    //e.g. a model with id 1
    static public IPoolable SpawnObject(IPoolable prefab, int poolId = 0) //e.g. a model with id 1
    {
        var filteredPoolList = poolList.Where(x => x.elementType == prefab.ElementType).FirstOrDefault();
        
        if(filteredPoolList != null)
        {
            var objectPool = filteredPoolList.objectPoolList.Where(x => x.poolId == poolId).FirstOrDefault();

            if (objectPool != null)
            {
                var poolable = objectPool.pool.Where(x => !x.IsActive).FirstOrDefault();
                
                if (poolable != null)
                {
                    return poolable;
                }
            }
        }

        var poolObject = prefab.Instantiate();
        poolObject.Transform.gameObject.AddComponent<GlobalComponent>();

        poolObject.PoolId = poolId;

        Add(poolObject);

        return poolObject;
    }

    static public void Add(IPoolable poolObject)
    {
        if (!poolList.Any(x => x.elementType == poolObject.ElementType))
            poolList.Add(new Pool(poolObject.ElementType));

        poolList.Where(x => x.elementType == poolObject.ElementType).FirstOrDefault().Add(poolObject);
    }

    static public void ClosePoolObject(IPoolable poolable)
    {
        poolable.ClosePoolable();

        poolable.Transform.SetParent(instance.transform, false);
    }
}
