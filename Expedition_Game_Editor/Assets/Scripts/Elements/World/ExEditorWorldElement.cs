using UnityEngine;
using System.Collections;
using System.Linq;

public class ExEditorWorldElement : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;
    
    private Vector3 position;
    private Vector3 rotation;
    
    private float scaleMultiplier;

    public EditorElement EditorElement   { get { return GetComponent<EditorElement>(); } }

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

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);

        switch (EditorElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;
            case Enums.DataType.Interaction:        SetInteractionElement();        break;
            case Enums.DataType.WorldObject:        SetWorldObjectElement();        break;
            case Enums.DataType.Phase:              SetPartyElement();              break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.GeneralData.DataType);    break;
        }

        transform.localPosition     = new Vector3(position.x, position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.data.elementData;
        
        var prefab      = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        position = new Vector3(elementData.positionX, elementData.positionY, elementData.positionZ);
        rotation = new Vector3(elementData.rotationX, elementData.rotationY, elementData.rotationZ);

        scaleMultiplier = elementData.scaleMultiplier;

        SetObjectGraphic();
    }

    private void SetInteractionElement()
    {
        var elementData = (InteractionElementData)EditorElement.DataElement.data.elementData;

        var prefab      = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scaleMultiplier = elementData.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetWorldObjectElement()
    {
        var elementData = (WorldObjectElementData)EditorElement.DataElement.data.elementData;

        var prefab      = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic   = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.ObjectGraphicId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scaleMultiplier = elementData.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetPartyElement()
    {
        var elementData = (PhaseElementData)EditorElement.DataElement.data.elementData;

        var prefab = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        position = new Vector3(elementData.DefaultPositionX, elementData.DefaultPositionY, elementData.DefaultPositionZ);
        rotation = new Vector3(elementData.DefaultRotationX, elementData.DefaultRotationY, elementData.DefaultRotationZ);

        scaleMultiplier = elementData.DefaultScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetObjectGraphic()
    {
        objectGraphic.EditorElement = EditorElement;

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
        if (EditorElement.glow != null)
        {
            EditorElement.glow.SetActive(false);
            EditorElement.glow = null;
        }

        if (EditorElement.lockIcon != null)
        {
            EditorElement.lockIcon.SetActive(false);
            EditorElement.lockIcon = null;
        }
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
