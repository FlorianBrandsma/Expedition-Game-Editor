using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IPoolable
{
    public PoolManager.PoolType PoolType { get { return PoolManager.PoolType.Tile; } }

    public int Id { get; set; }
}
