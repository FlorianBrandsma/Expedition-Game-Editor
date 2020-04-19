using UnityEngine;
using System.Collections;
using System.Linq;

public class EditorWorldElement : MonoBehaviour, IElement
{
    private ObjectGraphic objectGraphic;
    
    private SelectionElement Element { get { return GetComponent<SelectionElement>(); } }

    public Color ElementColor
    {
        set
        {
            if (objectGraphic == null) return;

            objectGraphic.SetStatus(value);
        }
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
    }

    private void SetWorldInteractableElement()
    {
        var data = Element.data;
        var dataElement = (WorldInteractableDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.ObjectGraphicId, prefab.PoolType, prefab);

        transform.localPosition     = new Vector3(dataElement.startPosition.x + dataElement.positionX, dataElement.startPosition.y - dataElement.positionY, -dataElement.positionZ);
        transform.localEulerAngles  = new Vector3(dataElement.rotationX, dataElement.rotationY, dataElement.rotationZ);
        transform.localScale        = new Vector3(1 * dataElement.scaleMultiplier, 1 * dataElement.scaleMultiplier, 1 * dataElement.scaleMultiplier);

        SetObjectGraphic();
    }

    private void SetInteractionElement()
    {
        var data = Element.data;
        var dataElement = (InteractionDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.objectGraphicId, prefab.PoolType, prefab);
        
        transform.localPosition     = new Vector3(dataElement.startPosition.x + dataElement.PositionX, dataElement.startPosition.y - dataElement.PositionY, -dataElement.PositionZ);
        transform.localEulerAngles  = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);
        transform.localScale        = new Vector3(1 * dataElement.ScaleMultiplier, 1 * dataElement.ScaleMultiplier, 1 * dataElement.ScaleMultiplier);

        SetObjectGraphic();
    }

    private void SetWorldObjectElement()
    {
        var data = Element.data;
        var dataElement = (WorldObjectDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.ObjectGraphicId, prefab.PoolType, prefab);

        transform.localPosition     = new Vector3(dataElement.startPosition.x + dataElement.PositionX, dataElement.startPosition.y - dataElement.PositionY, -dataElement.PositionZ);
        transform.localEulerAngles  = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);
        transform.localScale        = new Vector3(1 * dataElement.ScaleMultiplier, 1 * dataElement.ScaleMultiplier, 1 * dataElement.ScaleMultiplier);

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
    }
}
