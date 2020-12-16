using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum AgentState
{
    Spawn,
    Idle,
    Move
}

public enum DestinationType
{
    Interaction,
    Scene
}

public class ExGameWorldAgent : MonoBehaviour, IGameElement, IElement, IPoolable
{
    private Vector3 position;
    private Vector3 rotation;
    private float scale;

    private float speed;

    public GameElement GameElement              { get { return GetComponent<GameElement>(); } }
    
    private NavMeshAgent Agent                  { get { return GetComponent<NavMeshAgent>(); } }
    private SphereCollider InteractionCollider  { get { return GetComponent<SphereCollider>(); } }
    public Animator Animator                    { get { return GameElement.Model.Animator; } }

    private bool Moving                         { get { return Agent.velocity.x > 0.1f || Agent.velocity.z > 0.1f; } }

    public Color ElementColor                   { set { } }

    public Transform Transform                  { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType        { get { return Enums.ElementType.GameWorldAgent; } }
    public int Id                               { get; set; }
    public bool IsActive                        { get { return gameObject.activeInHierarchy; } }

    public GameWorldInteractableElementData GameWorldInteractableElementData { get { return (GameWorldInteractableElementData)GameElement.DataElement.ElementData; } }

    public AgentState AgentState
    {
        get { return GameWorldInteractableElementData.AgentState; }
        set { GameWorldInteractableElementData.AgentState = value; }
    }

    public DestinationType DestinationType
    {
        get { return GameWorldInteractableElementData.DestinationType; }
        set { GameWorldInteractableElementData.DestinationType = value; }
    }
    
    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        Agent.updatePosition = false;
        
        return newElement;
    }

    public void InitializeElement()
    {
        AgentState = AgentState.Spawn;
    }
    
    public void SetElement()
    {
        SetAgent();
    }

    private void SetAgent()
    {
        if (GameElement.Model != null)
            GameElement.Model.gameObject.SetActive(false);

        switch (GameWorldInteractableElementData.Type)
        {
            case Enums.InteractableType.Agent:          SetGameWorldInteractableAgent();        break;
            case Enums.InteractableType.Controllable:   SetGameWorldInteractableControllable(); break;

            default: Debug.Log("CASE MISSING: " + GameWorldInteractableElementData.Type); break;
        }

        SetModel();

        transform.localPosition     = new Vector3(position.x, position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);

        transform.localScale = new Vector3(scale, scale, scale);

        Agent.speed = speed;

        if (GameWorldInteractableElementData.Type == Enums.InteractableType.Agent && AgentState == AgentState.Spawn)
        {
            StartCoroutine(WakeAgent());
        }
    }
    
    private void SetGameWorldInteractableAgent()
    {
        var prefab = Resources.Load<Model>(GameWorldInteractableElementData.ModelPath);
        GameElement.Model = (Model)PoolManager.SpawnObject(prefab, GameWorldInteractableElementData.ModelId);

        var interactionData = GameWorldInteractableElementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveDestination;

        position = GameWorldInteractableElementData.DestinationPosition;
        rotation = GameWorldInteractableElementData.ArrivalRotation;

        scale = GameWorldInteractableElementData.Scale;

        speed = GameWorldInteractableElementData.Speed;

        InteractionCollider.radius = GameWorldInteractableElementData.Interaction.InteractionRange;
    }

    private void SetGameWorldInteractableControllable()
    {
        var prefab = Resources.Load<Model>(GameWorldInteractableElementData.ModelPath);
        GameElement.Model = (Model)PoolManager.SpawnObject(prefab, GameWorldInteractableElementData.ModelId);

        if (GameWorldInteractableElementData.Id == GameManager.instance.ActiveWorldInteractableControllableId)
        {
            var playerData = GameManager.instance.gameSaveData.PlayerSaveData;
            
            position = new Vector3(playerData.PositionX, playerData.PositionY, playerData.PositionZ);
            rotation = Vector3.zero;

            scale = GameWorldInteractableElementData.Scale;

            Agent.avoidancePriority = 0;
        }

        PlayerControlManager.instance.SetWorldInteractableControllableTerrainTileId(GameWorldInteractableElementData);

        speed = GameWorldInteractableElementData.Speed;
    }

    private void SetModel()
    {
        GameElement.Model.transform.SetParent(transform, false);

        GameElement.Model.gameObject.SetActive(true);
    }

    private IEnumerator WakeAgent()
    {
        yield return new WaitForSecondsRealtime(1);

        AgentState = AgentState.Idle;

        SetDestination();
    }

    public void UpdateElement()
    {
        SetDestination();
    }

    private void SetDestination()
    {
        var worldInteractableElementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        switch (worldInteractableElementData.Type)
        {
            case Enums.InteractableType.Agent:          SetAgentDestination();          break;
            case Enums.InteractableType.Controllable:   SetControllableDestination();   break;

            default: Debug.Log("CASE MISSING: " + GameWorldInteractableElementData.Type); break;
        }

        if (!Agent.isOnNavMesh) return;

        //Stop coroutines e.g. rotation
        StopAllCoroutines();

        var destination = new Vector3(position.x, position.y, -position.z);

        if (worldInteractableElementData.ArriveInstantly)
        {
            transform.position = destination;
            transform.eulerAngles = rotation;
        }
        
        Agent.destination = destination;

        //Immediately arrive at destination if the new destination distance is too short
        if (AgentState == AgentState.Idle && Vector3.Distance(transform.position, Agent.destination) <= Agent.stoppingDistance)
        {
            ArriveDestination();
        }
    }

    private void SetAgentDestination()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        position = elementData.DestinationPosition;
        rotation = elementData.ArrivalRotation;
    }

    private void SetControllableDestination()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        if (elementData.Id == GameManager.instance.ActiveWorldInteractableControllableId) return;

        //{
        //    var playerSaveData = GameManager.instance.gameSaveData.playerSaveData;
        //    position = new Vector3(playerSaveData.PositionX, transform.localPosition.y, playerSaveData.PositionZ);
        //}
    }
    
    private void Update()
    {
        if (!Agent.isOnNavMesh) return;

        var worldInteractableElementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        switch (worldInteractableElementData.Type)
        {
            case Enums.InteractableType.Agent:          UpdateAgent();          break;
            case Enums.InteractableType.Controllable:   UpdateControllable();   break;

            default: Debug.Log("CASE MISSING: " + GameWorldInteractableElementData.Type); break;
        }
    }

    private void UpdateAgent()
    {
        if (AgentState == AgentState.Idle && Vector3.Distance(gameObject.transform.position, Agent.destination) >= Agent.stoppingDistance)
        {
            AgentState = AgentState.Move;
        }

        GameWorldInteractableElementData.CurrentPosition = new Vector3(transform.position.x, transform.position.y, -transform.position.z);

        //Settle the agent in place when it is close to the destination but stopped moving (due to potential agent clashing)
        if (AgentState == AgentState.Move && !Moving && Vector3.Distance(gameObject.transform.position, Agent.destination) < Agent.stoppingDistance)
            ArriveDestination();
    }

    private void UpdateControllable() { }

    private void ArriveDestination()
    {
        switch (GameWorldInteractableElementData.Type)
        {
            case Enums.InteractableType.Agent: ArriveGameWorldInteractableAgent(); break;
            case Enums.InteractableType.Controllable: break;

            default: Debug.Log("CASE MISSING: " + GameWorldInteractableElementData.Type); break;
        }
    }

    private void ArriveGameWorldInteractableAgent()
    {
        MovementManager.Arrive(GameWorldInteractableElementData);

        if (GameWorldInteractableElementData.AllowRotation)
        {
            StopAllCoroutines();
            StartCoroutine(Rotate(rotation.y));
        }
    }

    private IEnumerator Rotate(float angle)
    {
        var rotateSpeed = 10;

        while (Mathf.Abs(transform.rotation.eulerAngles.y - angle) > 1f)
        {
            //Take destination's patience into account: rotation must always be stopped when patience runs out
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z), rotateSpeed * TimeManager.instance.TimeScale * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameWorldInteractableElementData.Type != Enums.InteractableType.Controllable) return;

        var colliderWorldInteractableElementData = (GameWorldInteractableElementData)other.GetComponent<GameElement>().DataElement.ElementData;

        PlayerControlManager.instance.potentialTargetList.Add(colliderWorldInteractableElementData);
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameWorldInteractableElementData.Type != Enums.InteractableType.Controllable) return;

        var colliderWorldInteractableElementData = (GameWorldInteractableElementData)other.GetComponent<GameElement>().DataElement.ElementData;

        InteractionManager.CancelInteractionOnRange(colliderWorldInteractableElementData);

        PlayerControlManager.instance.RemoveSelectionTarget(colliderWorldInteractableElementData);
    }

    private void StopAgent()
    {
        if (!Agent.isOnNavMesh) return;

        Agent.isStopped = true;

        Agent.ResetPath();
    }
    
    public void CloseElement()
    {
        StopAgent();
        
        StopAllCoroutines();
        
        position = new Vector3();
        rotation = new Vector3();

        scale = 0;

        if(GameWorldInteractableElementData.Type == Enums.InteractableType.Agent)
        {
            PlayerControlManager.instance.RemoveSelectionTarget(GameWorldInteractableElementData);
        }

        GameElement.CloseElement();
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
