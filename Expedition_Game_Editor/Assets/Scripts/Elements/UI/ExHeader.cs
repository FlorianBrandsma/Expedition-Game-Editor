using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExHeader : MonoBehaviour, IPoolable
{
    public Text label;

    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.Header; } }
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
