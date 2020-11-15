using UnityEngine;

public class ExEditorWorldElement : MonoBehaviour, IElement, IPoolable
{
    private Model model;
    
    private Vector3 position;
    private Vector3 rotation;
    
    private float scale;

    public EditorElement EditorElement      { get { return GetComponent<EditorElement>(); } }

    public Color ElementColor
    {
        set
        {
            if (model == null) return;

            model.SetStatus(value);
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
        if (model != null)
            model.gameObject.SetActive(false);

        switch (EditorElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.WorldInteractable:      SetWorldInteractableElement();      break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationElement(); break;
            case Enums.DataType.WorldObject:            SetWorldObjectElement();            break;
            case Enums.DataType.Phase:                  SetPhaseElement();                  break;
            case Enums.DataType.SceneActor:             SetSceneActorElement();             break;
            case Enums.DataType.SceneProp:              SetScenePropElement();              break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.ElementData.DataType);    break;
        }

        transform.localPosition     = new Vector3(position.x, position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(scale, scale, scale);
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetInteractionDestinationElement()
    {
        var elementData = (InteractionDestinationElementData)EditorElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetWorldObjectElement()
    {
        var elementData = (WorldObjectElementData)EditorElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetPhaseElement()
    {
        var elementData = (PhaseElementData)EditorElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.DefaultPositionX, elementData.DefaultPositionY, elementData.DefaultPositionZ);
        rotation = new Vector3(elementData.DefaultRotationX, elementData.DefaultRotationY, elementData.DefaultRotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetSceneActorElement()
    {
        var elementData = (SceneActorElementData)EditorElement.DataElement.ElementData;

        var prefab = Resources.Load<Model>(elementData.ModelPath);
        model = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetScenePropElement()
    {
        var elementData = (ScenePropElementData)EditorElement.DataElement.ElementData;

        var prefab = Resources.Load<Model>(elementData.ModelPath);
        model = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetModel()
    {
        model.EditorElement = EditorElement;

        model.transform.SetParent(transform, false);

        model.gameObject.SetActive(true);
    }

    public void CloseElement()
    {
        if (model == null) return;

        PoolManager.ClosePoolObject(model);
        model = null;

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
        //GameElement.DataElement.data.elementData.DataElement = null;
        //GameElement.DataElement.data.elementData = null;

        //if(EditorElement.DataElement.ElementData.DataType == Enums.DataType.InteractionDestination)
        //{
        //    var elementData = (InteractionDestinationElementData)EditorElement.DataElement.ElementData;

        //    Debug.Log(elementData.DebugName + elementData.Id);
        //}

        gameObject.SetActive(false);
    }
}
