using UnityEngine;
using UnityEngine.UI;

public class ExDropdown : MonoBehaviour, IPoolable
{
    public Dropdown Dropdown                { get { return GetComponent<Dropdown>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.Dropdown; } }
    public int PoolId                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
