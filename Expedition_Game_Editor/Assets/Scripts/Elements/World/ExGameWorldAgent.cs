using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

public enum AgentState
{
    Spawn,
    Idle,
    Move
}

public class ExGameWorldAgent : MonoBehaviour, IElement, IPoolable
{
    private Model model;

    private Vector3 position;
    private Vector3 rotation;
    private float scale;

    private float speed;

    public GameElement GameElement          { get { return GetComponent<GameElement>(); } }

    public NavMeshAgent Agent               { get { return GetComponent<NavMeshAgent>(); } }
    public Animator Animator                { get { return model.Animator; } }
    
    private bool Moving                     { get { return Agent.velocity.x > 0.1f || Agent.velocity.z > 0.1f; } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.GameWorldAgent; } }
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
        if (model != null)
            model.gameObject.SetActive(false);

        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableAgent();    break;
            case Enums.DataType.GamePartyMember:        SetGamePartyMemberAgent();          break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }

        transform.localPosition = new Vector3(position.x, position.y, -position.z);
        transform.localEulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);

        transform.localScale = new Vector3(scale, scale, scale);

        Agent.speed = speed;

        if (AgentState == AgentState.Spawn)
        {
            StartCoroutine(WakeAgent());
        }
    }
    
    private void SetGameWorldInteractableAgent()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        var prefab = Resources.Load<Model>(elementData.ModelPath);
        model = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        var interactionData = elementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveDestination;
        
        position = interactionDestinationData.Position;
        rotation = new Vector3(interactionDestinationData.RotationX, interactionDestinationData.RotationY, interactionDestinationData.RotationZ);

        scale = elementData.Scale;

        speed = elementData.Speed;

        SetModel();
    }

    private void SetGamePartyMemberAgent()
    {
        var elementData = (GamePartyMemberElementData)GameElement.DataElement.ElementData;

        var prefab  = Resources.Load<Model>(elementData.ModelPath);
        model       = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        if (elementData.Id == GameManager.instance.ActivePartyMemberId)
        {
            var playerData = GameManager.instance.gameSaveData.PlayerSaveData;

            position = new Vector3(playerData.PositionX, playerData.PositionY, playerData.PositionZ);
            rotation = Vector3.zero;

            scale = elementData.Scale;

            Agent.avoidancePriority = 0;
        }

        speed = elementData.Speed;

        SetModel();
    }

    private void SetModel()
    {
        model.transform.SetParent(transform, false);

        model.gameObject.SetActive(true);
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
        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableDestination();  break;
            case Enums.DataType.GamePartyMember:        SetGamePartyMemberDestination();        break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }

        if (!Agent.isOnNavMesh) return;

        Agent.destination = new Vector3(position.x, position.y, -position.z);

        //Immediately arrive at destination if the new destination distance is too short
        if (AgentState == AgentState.Idle && Vector3.Distance(gameObject.transform.position, Agent.destination) <= Agent.stoppingDistance)
        {
            ArriveDestination();
        }
    }

    private void SetGameWorldInteractableDestination()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;
        
        var interactionDestinationData = elementData.ActiveInteraction.ActiveDestination;

        //elementData.Position = new Vector3()
        position = interactionDestinationData.Position;
    }

    private void SetGamePartyMemberDestination()
    {
        var elementData = (GamePartyMemberElementData)GameElement.DataElement.ElementData;

        if (elementData.Id == GameManager.instance.ActivePartyMemberId) return;

        //{
        //    var playerSaveData = GameManager.instance.gameSaveData.playerSaveData;
        //    position = new Vector3(playerSaveData.PositionX, transform.localPosition.y, playerSaveData.PositionZ);
        //}
    }
    
    private void Update()
    {
        if (!Agent.isOnNavMesh) return;
        
        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  UpdateGameWorldInteractableAgent(); break;
            case Enums.DataType.GamePartyMember:        UpdateGamePartyMemberAgent();       break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }
    }

    private void UpdateGameWorldInteractableAgent()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        if (AgentState == AgentState.Idle && Vector3.Distance(gameObject.transform.position, Agent.destination) >= Agent.stoppingDistance)
        {
            AgentState = AgentState.Move;
        }

        elementData.Position = new Vector3(transform.position.x, transform.position.y, -transform.position.z);

        //Settle the agent in place when it is close to the destination but stopped moving (due to potential agent clashing)
        if (AgentState == AgentState.Move && !Moving && Vector3.Distance(gameObject.transform.position, Agent.destination) < Agent.stoppingDistance)
            ArriveDestination();
    }

    private void UpdateGamePartyMemberAgent() { }

    private void ArriveDestination()
    {
        //This belongs in the movement manager, or at least a version of it
        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable: ArriveGameWorldInteractableAgent(); break;
            case Enums.DataType.GamePartyMember: break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }
    }

    private void ArriveGameWorldInteractableAgent()
    {
        var worldInteractableElementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        var destinationData = worldInteractableElementData.ActiveInteraction.ActiveDestination;

        MovementManager.Arrive(worldInteractableElementData);

        if (!destinationData.FreeRotation && destinationData.Patience > 0)
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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z), rotateSpeed * TimeManager.gameTimeSpeed * Time.deltaTime);
            yield return null;
        }
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

        GameElement.DataElement.ElementData.DataElement = null;
        
        position = new Vector3();
        rotation = new Vector3();

        scale = 0;

        if (model == null) return;

        PoolManager.ClosePoolObject(model);
        model = null;
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
