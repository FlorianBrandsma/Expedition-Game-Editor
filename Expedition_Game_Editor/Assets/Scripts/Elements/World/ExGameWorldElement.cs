using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class ExGameWorldElement : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;

    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;

    private NavMeshObstacleShape shape;
    private Vector3 center;
    private Vector3 size;
    private float radius;
    private float height;

    private float scaleMultiplier;
    
    public NavMeshObstacle Obstacle         { get { return GetComponent<NavMeshObstacle>(); } }

    public GameElement GameElement          { get { return GetComponent<GameElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.GameWorldElement; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement()
    {
        SetElement();
        GameElement.UpdateElement();
    }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);

        switch (GameElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.GameWorldObject:        SetGameWorldObjectElement();        break;
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableElement();  break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.GeneralData.DataType); break;
        }
        
        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);

        SetObstacle();
    }

    private void SetGameWorldObjectElement()
    {
        var elementData = (GameWorldObjectElementData)GameElement.DataElement.data.elementData;

        var prefab = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        position = new Vector3(elementData.positionX, elementData.positionY, elementData.positionZ);
        rotation = new Vector3(elementData.rotationX, elementData.rotationY, elementData.rotationZ);

        scaleMultiplier = elementData.scaleMultiplier;
        
        SetObjectGraphic();
    }

    private void SetGameWorldInteractableElement()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.data.elementData;

        var prefab = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        var interactionData = elementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveInteractionDestination;

        position = new Vector3(interactionDestinationData.positionX, interactionDestinationData.positionY, interactionDestinationData.positionZ);
        rotation = new Vector3(interactionDestinationData.rotationX, interactionDestinationData.rotationY, interactionDestinationData.rotationZ);

        scaleMultiplier = elementData.scaleMultiplier;

        SetObjectGraphic();
    }

    private void SetObjectGraphic()
    {
        objectGraphic.transform.SetParent(transform, false);

        objectGraphic.gameObject.SetActive(true);
    }

    private void SetObstacle()
    {
        switch(objectGraphic.obstacleShape)
        {
            case NavMeshObstacleShape.Box:      SetBoxObstacle();       break;
            case NavMeshObstacleShape.Capsule:  SetCapsuleObstacle();   break;
        }
    }

    private void SetBoxObstacle()
    {
        var collider = objectGraphic.GetComponent<BoxCollider>();

        Obstacle.shape = objectGraphic.obstacleShape;

        Obstacle.size = collider.size;
        Obstacle.center = collider.center;
    }

    private void SetCapsuleObstacle()
    {
        var collider = objectGraphic.GetComponent<CapsuleCollider>();

        Obstacle.shape = objectGraphic.obstacleShape;

        Obstacle.center = collider.center;
        Obstacle.radius = collider.radius;
        Obstacle.height = collider.height;
    }

    public void CloseElement()
    {
        GameElement.DataElement.data.elementData.DataElement = null;
        GameElement.DataElement.data.elementData = null;

        if (objectGraphic == null) return;

        PoolManager.ClosePoolObject(objectGraphic);
        objectGraphic = null;
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
