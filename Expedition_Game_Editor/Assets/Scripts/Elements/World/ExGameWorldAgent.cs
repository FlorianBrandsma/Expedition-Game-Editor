﻿using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

public class ExGameWorldAgent : MonoBehaviour, IElement, IPoolable
{
    private Model model;
    public Animator Animator { get { return model.Animator; } }

    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;
    
    private float scale;

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
    
    public void SetElement()
    {
        if (model != null)
            model.gameObject.SetActive(false);
        
        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableAgent();    break;
            case Enums.DataType.GamePartyMember:        SetGamePartyMemberAgent();          break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }
        
        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);

        transform.localScale        = new Vector3(scale, scale, scale);

        Agent.speed = speed;
    }
    
    private void SetGameWorldInteractableAgent()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        var prefab = Resources.Load<Model>(elementData.ModelPath);
        model = (Model)PoolManager.SpawnObject(prefab, elementData.ModelId);

        var interactionData = elementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveInteractionDestination;
        
        position = new Vector3(interactionDestinationData.PositionX, interactionDestinationData.PositionY, interactionDestinationData.PositionZ);
        rotation = new Vector3(interactionDestinationData.RotationX, interactionDestinationData.RotationY, interactionDestinationData.RotationZ);

        scale = elementData.Scale;

        speed = elementData.Speed;

        SetModel();

        ArriveDestination();
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

    public void UpdateElement()
    {
        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable:  SetGameWorldInteractableDestination();  break;
            case Enums.DataType.GamePartyMember:        SetGamePartyMemberDestination();        break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }

        if (!Agent.isOnNavMesh) return;

        Agent.destination = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);
    }

    private void SetGameWorldInteractableDestination()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;

        var interactionData = elementData.ActiveInteraction;
        var interactionDestinationData = interactionData.ActiveInteractionDestination;

        position = new Vector3(interactionDestinationData.PositionX + Random.Range(-interactionDestinationData.PositionVariance, interactionDestinationData.PositionVariance), 
                               interactionDestinationData.PositionY, 
                               interactionDestinationData.PositionZ + Random.Range(-interactionDestinationData.PositionVariance, interactionDestinationData.PositionVariance));
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

    private void SetModel()
    {
        model.transform.SetParent(transform, false);

        model.gameObject.SetActive(true);
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
        Agent.destination = new Vector3(startPosition.x + position.x, startPosition.y + position.y, -position.z);

        if(!allowMoving && !Moving)
        {
            if (Agent.remainingDistance >= Agent.stoppingDistance)
            {
                allowMoving = true;

                //TEMPORARY MEASURE TO PREVENT ERRORS WHILE OBJECTS LIKE POOLS ARE SPAWNED AS AGENTS
                //if(Animator != null)
                //{
                //    Animator.SetBool("IsMoving", true);
                //    Animator.SetFloat("MoveSpeedSensitivity", Agent.speed / scale);
                //}

            } else {

                //Immediately arrive at destination if the new destination distance is too short
                ArriveDestination();
            }
        }
        
        //Settle the agent in place when it is close to the destination but stopped moving (due to potential agent clashing)
        if (allowMoving && Agent.remainingDistance < Agent.stoppingDistance && !Moving)
            ArriveDestination();
    }

    private void UpdateGamePartyMemberAgent() { }

    private void ArriveDestination()
    {
        allowMoving = false;
        
        //TEMPORARY MEASURE TO PREVENT ERRORS WHILE OBJECTS LIKE POOLS ARE SPAWNED AS AGENTS
        //if (Animator != null)
        //{
        //    Animator.SetBool("IsMoving", false);
        //}

        switch (GameElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.GameWorldInteractable: ArriveGameWorldInteractableAgent(); break;
            case Enums.DataType.GamePartyMember: break;

            default: Debug.Log("CASE MISSING: " + GameElement.DataElement.ElementData.DataType); break;
        }
    }

    private void ArriveGameWorldInteractableAgent()
    {
        var elementData = (GameWorldInteractableElementData)GameElement.DataElement.ElementData;
        var interactionData = elementData.ActiveInteraction;
        var destinationData = interactionData.ActiveInteractionDestination;

        interactionData.Arrived = true;

        //Debug.Log("Agent has arrived");
        if (!destinationData.FreeRotation && destinationData.Patience > 0)
        {
            StopAllCoroutines();
            StartCoroutine(Rotate(rotation.y));
        }
    }

    private void StopAgent()
    {
        if (!Agent.isOnNavMesh) return;

        allowMoving = false;

        //TEMPORARY MEASURE TO PREVENT ERRORS WHILE OBJECTS LIKE POOLS ARE SPAWNED AS AGENTS
        //if (Animator != null)
        //{
        //    Animator.SetBool("IsMoving", false);
        //}

        StopAllCoroutines();

        Agent.isStopped = true;
        Agent.ResetPath();
    }

    IEnumerator Rotate(float angle)
    {
        var rotateSpeed = 10;

        while(Mathf.Abs(transform.rotation.eulerAngles.y - angle) > 1f)
        {
            //Take destination's patience into account: rotation must always be stopped when patience runs out
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z), rotateSpeed * TimeManager.gameTimeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void CloseElement()
    {
        StopAgent();
        
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
