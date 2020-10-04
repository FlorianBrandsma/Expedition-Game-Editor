using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ExGameWorldElement : MonoBehaviour, IElement, IPoolable
{
    private Model model;

    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;

    private NavMeshObstacleShape shape;
    private Vector3 center;
    private Vector3 size;
    private float radius;
    private float height;

    private float scale;
    
    public NavMeshObstacle Obstacle         { get { return GetComponent<NavMeshObstacle>(); } }

    public GameElement GameElement          { get { return GetComponent<GameElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.GameWorldElement; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public AgentState AgentState
    {
        get
        {
            switch (GameElement.DataElement.ElementData.DataType)
            {
                case Enums.DataType.GameWorldInteractable:
                    return ((GameWorldInteractableElementData)GameElement.DataElement.ElementData).AgentState;

                default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); return AgentState.Idle;
            }
        }
        set
        {
            switch (GameElement.DataElement.ElementData.DataType)
            {
                case Enums.DataType.GameWorldInteractable:

                    ((GameWorldInteractableElementData)GameElement.DataElement.ElementData).AgentState = value;

                    break;

                default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
            }
        }
    }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        return newElement;
    }

    public void InitializeElement()
    {
        if(GameElement.DataElement.ElementData.DataType == Enums.DataType.GameWorldInteractable)
            AgentState = AgentState.Idle;
    }

    public void UpdateElement()
    {
        SetElement();
        GameElement.UpdateElement();
    }

    public void SetElement()
    {
        if (model != null)
            model.gameObject.SetActive(false);

        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldObject:        SetGameWorldObjectElement();        break;
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableElement();  break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }
        
        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scale, 1 * scale, 1 * scale);

        SetObstacle();
    }

    private void SetGameWorldObjectElement()
    {
        var elementData = (GameWorldObjectElementData)GameElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        Obstacle.carving = true;

        SetModel();
    }

    private void SetGameWorldInteractableElement()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        var interactionData = elementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveDestination;

        position = new Vector3(interactionDestinationData.PositionX, interactionDestinationData.PositionY, interactionDestinationData.PositionZ);
        rotation = new Vector3(interactionDestinationData.RotationX, interactionDestinationData.RotationY, interactionDestinationData.RotationZ);

        scale = elementData.Scale;

        Obstacle.carving = false;

        SetModel();
    }

    private void SetModel()
    {
        model.transform.SetParent(transform, false);

        model.gameObject.SetActive(true);
    }

    private void SetObstacle()
    {
        switch(model.obstacleShape)
        {
            case NavMeshObstacleShape.Box:      SetBoxObstacle();       break;
            case NavMeshObstacleShape.Capsule:  SetCapsuleObstacle();   break;
        }
    }

    private void SetBoxObstacle()
    {
        var collider = model.GetComponent<BoxCollider>();
        
        Obstacle.shape = model.obstacleShape;

        Obstacle.size = collider.size;
        Obstacle.center = collider.center;
    }

    private void SetCapsuleObstacle()
    {
        var collider = model.GetComponent<CapsuleCollider>();

        Obstacle.shape = model.obstacleShape;

        Obstacle.center = collider.center;
        Obstacle.radius = collider.radius;
        Obstacle.height = collider.height;
    }

    public void CloseElement()
    {
        GameElement.DataElement.ElementData.DataElement = null;

        if (model == null) return;

        PoolManager.ClosePoolObject(model);
        model = null;
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
