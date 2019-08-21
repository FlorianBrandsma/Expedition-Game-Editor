using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

static public class ObjectManager
{
    public enum PoolType
    {
        SceneInteractable,
        ObjectGraphic,
        Tile
    }

    //That which stores a list for each pool type
    public class PoolManager
    {
        public List<Pool> poolList = new List<Pool>();

        public void Add(IPoolable poolObject)
        {
            if (!poolList.Any(x => x.poolType == poolObject.PoolType))
                poolList.Add(new Pool(poolObject.PoolType));

            poolList.Where(x => x.poolType == poolObject.PoolType).FirstOrDefault().Add(poolObject);
        }
    }

    //e.g. all objectGraphics
    public class Pool 
    {
        public PoolType poolType;
        public List<ObjectPool> objectPoolList = new List<ObjectPool>();

        public Pool(PoolType poolType)
        {
            this.poolType = poolType;
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

    static public PoolManager poolManager = new PoolManager();

    //e.g. an objectGraphic with id 1
    static public IPoolable SpawnObject(int id, PoolType poolType, IPoolable prefab) //e.g. an objectGraphic with id 1
    {
        var poolList = poolManager.poolList.Where(x => x.poolType == poolType).FirstOrDefault();

        if(poolList != null)
        {
            var objectPool = poolList.objectPoolList.Where(x => x.id == id).FirstOrDefault();

            if(objectPool != null)
            {
                switch(poolType)
                {
                    case PoolType.ObjectGraphic:

                        var test = objectPool.pool.Where(x => !((ObjectGraphic)x).gameObject.activeInHierarchy).FirstOrDefault();
                        
                        if (test != null)
                            return test;

                        break;

                    case PoolType.Tile:

                        var test2 = objectPool.pool.Where(x => !((Tile)x).gameObject.activeInHierarchy).FirstOrDefault();

                        if (test2 != null)
                            return test2;

                        break;
                }
            }
        }

        IPoolable poolObject;

        switch (poolType)
        {
            case PoolType.ObjectGraphic:    poolObject = Object.Instantiate((ObjectGraphic)prefab); break;
            case PoolType.Tile:             poolObject = Object.Instantiate((Tile)prefab); break;

            default: poolObject = null; break;
        }
        
        poolObject.Id = id;

        poolManager.Add(poolObject);

        return poolObject;
    }
}
