using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectGraphic : MonoBehaviour, IPoolable
{
    public ObjectProperties.Pivot pivot;

    public ObjectManager.PoolType PoolType { get { return ObjectManager.PoolType.ObjectGraphic; } }

    public int Id { get; set; }
}