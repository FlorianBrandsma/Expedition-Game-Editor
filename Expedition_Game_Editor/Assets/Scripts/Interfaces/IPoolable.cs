using UnityEngine;
using System.Collections;

public interface IPoolable
{
    PoolManager.PoolType PoolType { get; }
    int Id { get; set; }
    bool IsActive { get; }
    IPoolable Instantiate();
    void ClosePoolable();
}
