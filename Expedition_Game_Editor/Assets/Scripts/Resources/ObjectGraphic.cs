using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectGraphic : MonoBehaviour, IPoolable
{
    public ObjectProperties.Pivot pivot;

    public Vector3 previewRotation;
    public Vector3 previewScale;

    public PoolManager.PoolType PoolType { get { return PoolManager.PoolType.ObjectGraphic; } }

    public int Id { get; set; }

    public bool IsActive { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}