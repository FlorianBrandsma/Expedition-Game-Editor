using UnityEngine;
using System.Collections;
using System.Linq;

public class ExEditorWorldElement : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;
    
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
            case Enums.DataType.Phase:              SetPartyElement();              break;

            default: Debug.Log("CASE MISSING: " + Element.GeneralData.DataType);    break;
        }

        transform.localPosition     = new Vector3(position.x, position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);
    }

    private void SetWorldInteractableElement()
    {
        var dataElement = (WorldInteractableDataElement)Element.data.dataElement;
        
        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(prefab, dataElement.objectGraphicId);

        position = new Vector3(dataElement.positionX, dataElement.positionY, dataElement.positionZ);
        rotation = new Vector3(dataElement.rotationX, dataElement.rotationY, dataElement.rotationZ);

        scaleMultiplier = dataElement.scaleMultiplier;

        SetObjectGraphic();
    }

    private void SetInteractionElement()
    {
        var dataElement = (InteractionDataElement)Element.data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(prefab, dataElement.objectGraphicId);

        position = new Vector3(dataElement.PositionX, dataElement.PositionY, dataElement.PositionZ);
        rotation = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);

        scaleMultiplier = dataElement.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetWorldObjectElement()
    {
        var dataElement = (WorldObjectDataElement)Element.data.dataElement;

        var prefab      = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(prefab, dataElement.ObjectGraphicId);

        position = new Vector3(dataElement.PositionX, dataElement.PositionY, dataElement.PositionZ);
        rotation = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);

        scaleMultiplier = dataElement.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetPartyElement()
    {
        var dataElement = (PhaseDataElement)Element.data.dataElement;

        var prefab = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, dataElement.objectGraphicId);

        position = new Vector3(dataElement.DefaultPositionX, dataElement.DefaultPositionY, dataElement.DefaultPositionZ);
        rotation = new Vector3(dataElement.DefaultRotationX, dataElement.DefaultRotationY, dataElement.DefaultRotationZ);

        scaleMultiplier = dataElement.DefaultScaleMultiplier;

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

        PoolManager.ClosePoolObject(objectGraphic);
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
        gameObject.SetActive(false);
    }
}
