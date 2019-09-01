using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectGraphic : MonoBehaviour, IPoolable
{
    public ObjectProperties.Pivot pivot;

    public PoolManager.PoolType PoolType { get { return PoolManager.PoolType.ObjectGraphic; } }

    public int Id { get; set; }
}