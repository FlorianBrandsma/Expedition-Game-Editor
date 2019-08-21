using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IPoolable
{
    public ObjectManager.PoolType PoolType { get { return ObjectManager.PoolType.Tile; } }

    public int Id { get; set; }
}
