using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

public class ExGameWorldAgent : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;
    public Animator Animator { get { return objectGraphic.Animator; } }

    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;
    
    private float scaleMultiplier;

    private float speed;

    public NavMeshAgent Agent               { get { return GetComponent<NavMeshAgent>(); } }
    public GameElement GameElement          { get { return GetComponent<GameElement>(); } }

    public bool allowMoving;

    private bool Moving                     { get { return Agent.velocity.x > 0.1f || Agent.velocity.z > 0.1f; } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.GameWorldAgent; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        Agent.updatePosition = false;
        
        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement()
    {
        switch (GameElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableDestination();  break;
            case Enums.DataType.GamePartyMember:        SetGamePartyMemberDestination();        break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.GeneralData.DataType); break;
        }

        if (!Agent.isOnNavMesh) return;

        Debug.Log("update destination");
        Agent.destination = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
    }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);

        switch (GameElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableAgent();    break;
            case Enums.DataType.GamePartyMember:        SetGamePartyMemberAgent();          break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.GeneralData.DataType); break;
        }
        
        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);

        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);

        Agent.speed = speed;
    }

    private void SetGameWorldInteractableAgent()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.data.elementData;

        var prefab = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        var interactionData = elementData.interactionDataList.Where(x => x.containsActiveTime).First();

        position = new Vector3(interactionData.positionX, interactionData.positionY, interactionData.positionZ);
        rotation = new Vector3(interactionData.rotationX, interactionData.rotationY, interactionData.rotationZ);

        scaleMultiplier = interactionData.scaleMultiplier;

        speed = elementData.speed;

        SetObjectGraphic();
    }

    private void SetGamePartyMemberAgent()
    {
        var elementData = (GamePartyMemberElementData)GameElement.DataElement.data.elementData;

        var prefab = Resources.Load<ObjectGraphic>(elementData.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(prefab, elementData.objectGraphicId);

        if (elementData.Id == GameManager.instance.ActivePartyMemberId)
        {
            var playerData = GameManager.instance.gameSaveData.playerSaveData;

            position = new Vector3(playerData.PositionX, playerData.PositionY, playerData.PositionZ);
            rotation = Vector3.zero;

            scaleMultiplier = playerData.ScaleMultiplier;

            Agent.avoidancePriority = 0;
        }

        speed = elementData.speed;

        SetObjectGraphic();
    }

    private void SetGameWorldInteractableDestination()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.data.elementData;

        var interactionData = elementData.interactionDataList.Where(x => x.containsActiveTime).First();

        position = new Vector3(interactionData.positionX, interactionData.positionY, interactionData.positionZ);
    }

    private void SetGamePartyMemberDestination()
    {
        var elementData = (GamePartyMemberElementData)GameElement.DataElement.data.elementData;

        if (elementData.Id == GameManager.instance.ActivePartyMemberId) return;

        //{
        //    var playerSaveData = GameManager.instance.gameSaveData.playerSaveData;

        //    position = new Vector3(playerSaveData.PositionX, transform.localPosition.y, playerSaveData.PositionZ);
        //}
    }

    private void SetObjectGraphic()
    {
        objectGraphic.transform.SetParent(transform, false);

        objectGraphic.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!Agent.isOnNavMesh) return;

        switch (GameElement.DataElement.GeneralData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  UpdateGameWorldInteractableAgent(); break;
            case Enums.DataType.GamePartyMember:        UpdateGamePartyMemberAgent();       break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.GeneralData.DataType); break;
        }
    }

    private void UpdateGameWorldInteractableAgent()
    {
        Agent.destination = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);

        if (Agent.remainingDistance > Agent.stoppingDistance && !Moving)
        {
            allowMoving = true;

            Animator.SetBool("IsMoving", true);
            Animator.SetFloat("MoveSpeedSensitivity", Agent.speed);
        }

        //Settle the agent in place when it is close to the destination but stopped moving (due to potential agent clashing)
        if (allowMoving && Agent.remainingDistance < Agent.stoppingDistance && !Moving)
            SettleAgent();
    }

    private void UpdateGamePartyMemberAgent() { }

    private void SettleAgent()
    {
        allowMoving = false;
        Animator.SetBool("IsMoving", false);
        
        StopAllCoroutines();
        StartCoroutine(Rotate(rotation.y));
    }

    private void StopAgent()
    {
        if (!Agent.isOnNavMesh) return;

        allowMoving = false;
        Animator.SetBool("IsMoving", false);

        StopAllCoroutines();

        Agent.isStopped = true;
        Agent.ResetPath();
    }

    IEnumerator Rotate(float angle)
    {
        var rotateSpeed = 10;

        while(Mathf.Abs(transform.rotation.eulerAngles.y - angle) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z), rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void CloseElement()
    {
        StopAgent();
        
        GameElement.DataElement.data.elementData.DataElement = null;
        GameElement.DataElement.data.elementData = null;

        position = new Vector3();
        rotation = new Vector3();

        scaleMultiplier = 0;

        if (objectGraphic == null) return;

        PoolManager.ClosePoolObject(objectGraphic);
        objectGraphic = null;
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
