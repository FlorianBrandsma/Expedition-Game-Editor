using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ExGameWorldElement : MonoBehaviour, IGameElement, IElement, IPoolable
{
    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;

    private NavMeshObstacleShape shape;
    private Vector3 center;
    private Vector3 size;
    private float radius;
    private float height;

    private float scale;

    public GameElement GameElement              { get { return GetComponent<GameElement>(); } }

    public NavMeshObstacle Obstacle             { get { return GetComponent<NavMeshObstacle>(); } }
    private SphereCollider InteractionCollider  { get { return GetComponent<SphereCollider>(); } }
    

    public Color ElementColor                   { set { } }

    public Transform Transform                  { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType        { get { return Enums.ElementType.GameWorldElement; } }
    public int Id                               { get; set; }
    public bool IsActive                        { get { return gameObject.activeInHierarchy; } }

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
        if (GameElement.Model != null)
            GameElement.Model.gameObject.SetActive(false);

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

        var prefab = Resources.Load<Model>(elementData.ModelPath);
        GameElement.Model = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        position = new Vector3(elementData.PositionX, elementData.PositionY, elementData.PositionZ);
        rotation = new Vector3(elementData.RotationX, elementData.RotationY, elementData.RotationZ);

        scale = elementData.Scale;

        SetModel();
    }

    private void SetGameWorldInteractableElement()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        var prefab = Resources.Load<Model>(elementData.ModelPath);
        GameElement.Model = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        var interactionData = elementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveDestination;

        position = new Vector3(interactionDestinationData.PositionX, interactionDestinationData.PositionY, interactionDestinationData.PositionZ);
        rotation = new Vector3(interactionDestinationData.RotationX, interactionDestinationData.RotationY, interactionDestinationData.RotationZ);

        scale = elementData.Scale;

        InteractionCollider.radius = elementData.Interaction.InteractionRange;

        SetModel();
    }

    private void SetModel()
    {
        GameElement.Model.transform.SetParent(transform, false);

        GameElement.Model.gameObject.SetActive(true);
    }

    private void SetObstacle()
    {
        switch(GameElement.Model.obstacleShape)
        {
            case NavMeshObstacleShape.Box:      SetBoxObstacle();       break;
            case NavMeshObstacleShape.Capsule:  SetCapsuleObstacle();   break;
        }
    }

    private void SetBoxObstacle()
    {
        var collider = GameElement.Model.GetComponent<BoxCollider>();
        
        Obstacle.shape = GameElement.Model.obstacleShape;

        Obstacle.size = collider.size;
        Obstacle.center = collider.center;
    }

    private void SetCapsuleObstacle()
    {
        var collider = GameElement.Model.GetComponent<CapsuleCollider>();

        Obstacle.shape = GameElement.Model.obstacleShape;

        Obstacle.center = collider.center;
        Obstacle.radius = collider.radius;
        Obstacle.height = collider.height;
    }

    public void CloseElement()
    {
        if(GameElement.DataElement.ElementData.DataType == Enums.DataType.GameWorldInteractable)
        {
            var gameWorldInteractableElementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;
            PlayerControlManager.instance.RemoveSelectionTarget(gameWorldInteractableElementData);
        }

        GameElement.CloseElement();
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
