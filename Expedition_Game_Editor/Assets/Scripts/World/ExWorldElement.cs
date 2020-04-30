using UnityEngine;
using System.Collections;
using System.Linq;

public class ExWorldElement : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;
    
    private Vector3 startPosition;

    private Vector3 position;
    private Vector3 rotation;
    
    private float scaleMultiplier;

    public SelectionElement Element         { get { return GetComponent<SelectionElement>(); } }

    public Color ElementColor
    {
        set
        {
            if (objectGraphic == null) return;

            objectGraphic.SetStatus(value);
        }
    }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.WorldElement; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.Element);

        return newElement;
    }

    public void InitializeElement() { }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);

        switch (Element.GeneralData.DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;
            case Enums.DataType.Interaction:        SetInteractionElement();        break;
            case Enums.DataType.WorldObject:        SetWorldObjectElement();        break;

            default: Debug.Log("CASE MISSING: " + Element.GeneralData.DataType);    break;
        }

        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y - position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);
    }

    private void SetWorldInteractableElement()
    {
        var data = Element.data;
        var dataElement = (WorldInteractableDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.ObjectGraphicId, prefab);

        startPosition = dataElement.startPosition;

        position = new Vector3(dataElement.positionX, dataElement.positionY, dataElement.positionZ);
        rotation = new Vector3(dataElement.rotationX, dataElement.rotationY, dataElement.rotationZ);

        scaleMultiplier = dataElement.scaleMultiplier;

        SetObjectGraphic();
    }

    private void SetInteractionElement()
    {
        var data = Element.data;
        var dataElement = (InteractionDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.objectGraphicId, prefab);

        startPosition = dataElement.startPosition;

        position = new Vector3(dataElement.PositionX, dataElement.PositionY, dataElement.PositionZ);
        rotation = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);

        scaleMultiplier = dataElement.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetWorldObjectElement()
    {
        var data = Element.data;
        var dataElement = (WorldObjectDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.ObjectGraphicId, prefab);

        startPosition = dataElement.startPosition;

        position = new Vector3(dataElement.PositionX, dataElement.PositionY, dataElement.PositionZ);
        rotation = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);

        scaleMultiplier = dataElement.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetObjectGraphic()
    {
        objectGraphic.selectionElement = Element;

        objectGraphic.transform.SetParent(transform, false);

        objectGraphic.gameObject.SetActive(true);
    }

    public void CloseElement()
    {
        if (objectGraphic == null) return;

        objectGraphic.gameObject.SetActive(false);
        objectGraphic = null;

        CloseStatusIcons();
    }

    public void CloseStatusIcons()
    {
        if (Element.glow != null)
        {
            Element.glow.SetActive(false);
            Element.glow = null;
        }

        if (Element.lockIcon != null)
        {
            Element.lockIcon.SetActive(false);
            Element.lockIcon = null;
        }
    }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
