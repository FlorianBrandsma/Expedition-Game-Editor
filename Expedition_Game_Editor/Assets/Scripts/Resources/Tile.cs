using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, IPoolable
{
    public Transform Transform { get { return GetComponent<Transform>(); } }

    public Enums.ElementType ElementType { get { return Enums.ElementType.Tile; } }

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
