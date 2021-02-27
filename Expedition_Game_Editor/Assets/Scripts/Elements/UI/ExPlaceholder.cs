using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPlaceholder : MonoBehaviour, IElement, IPoolable
{
    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public EditorElement EditorElement      { get { return GetComponent<EditorElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.Placeholder; } }
    public int PoolId                       { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement() { }

    public void SetElement() { }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
