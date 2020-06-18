using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    //e.g. all objectGraphics
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
            if (!objectPoolList.Any(x => x.id == poolObject.Id))
                objectPoolList.Add(new ObjectPool(poolObject.Id));

            objectPoolList.Where(x => x.id == poolObject.Id).FirstOrDefault().pool.Add(poolObject);
        }
    }

    //e.g. all objectGraphics with id 1
    public class ObjectPool
    {
        public int id;
        public List<IPoolable> pool = new List<IPoolable>();

        public ObjectPool(int id)
        {
            this.id = id;
        }
    }

    static public PoolManager instance;

    static public List<Pool> poolList = new List<Pool>();

    private void Awake()
    {
        instance = this;
    }

    //e.g. an objectGraphic with id 1
    static public IPoolable SpawnObject(IPoolable prefab, int id = 0) //e.g. an objectGraphic with id 1
    {
        var filteredPoolList = poolList.Where(x => x.elementType == prefab.ElementType).FirstOrDefault();
        
        if(filteredPoolList != null)
        {
            var objectPool = filteredPoolList.objectPoolList.Where(x => x.id == id).FirstOrDefault();

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

        poolObject.Id = id;

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
