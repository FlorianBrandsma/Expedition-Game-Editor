using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExSlider : MonoBehaviour, IPoolable
{
    public Enums.ElementType elementType;

    public Slider Slider                    { get { return GetComponent<Slider>(); } }
    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
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
