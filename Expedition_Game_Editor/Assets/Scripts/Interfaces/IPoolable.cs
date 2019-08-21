using UnityEngine;
using System.Collections;

public interface IPoolable
{
    ObjectManager.PoolType PoolType { get; }
    int Id { get; set; }
}
