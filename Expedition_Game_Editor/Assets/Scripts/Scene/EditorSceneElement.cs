using UnityEngine;
using System.Collections;

public class EditorSceneElement : MonoBehaviour, IElement
{
    private ObjectGraphic objectGraphic;

    private SelectionElement Element { get { return GetComponent<SelectionElement>(); } }
    
    public void InitializeElement()
    {
        //properties = (PanelProperties)Element.DisplayManager.Display.Properties;
    }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);
        
        switch (Element.GeneralData.dataType)
        {
            case Enums.DataType.SceneInteractable:  SetSceneInteractableElement();  break;
            case Enums.DataType.Interaction:        SetInteractionElement();        break;
            case Enums.DataType.SceneObject:        SetSceneObjectElement();        break;

            default: Debug.Log("CASE MISSING: " + Element.GeneralData.dataType);    break;
        }
    }

    private void SetSceneInteractableElement()
    {
        var data = Element.data;
        var dataElement = (SceneInteractableDataElement)data.dataElement;

        var prefab = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(dataElement.objectGraphicId, prefab.PoolType, prefab);

        transform.localPosition = new Vector3(dataElement.startPosition.x + dataElement.positionX, dataElement.startPosition.y - dataElement.positionY, -dataElement.positionZ);
        transform.localEulerAngles = new Vector3(dataElement.rotationX, dataElement.rotationY, dataElement.rotationZ);
        transform.localScale = new Vector3(1, 1, 1);

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
        transform.localScale        = new Vector3(1, 1, 1);

        SetObjectGraphic();
    }

    private void SetSceneObjectElement()
    {
        var data = Element.data;
        var dataElement = (SceneObjectDataElement)data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(dataElement.ObjectGraphicId, prefab.PoolType, prefab);
        
        transform.localPosition     = new Vector3(dataElement.startPosition.x + dataElement.PositionX, dataElement.startPosition.y - dataElement.PositionY, -dataElement.PositionZ);
        transform.localEulerAngles  = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);
        transform.localScale        = new Vector3(1, 1, 1);

        SetObjectGraphic();
    }

    private void SetObjectGraphic()
    {
        objectGraphic.transform.SetParent(transform, false);

        objectGraphic.gameObject.SetActive(true);
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void CloseElement()
    {
        objectGraphic.gameObject.SetActive(false);
        objectGraphic = null;
    }
}
