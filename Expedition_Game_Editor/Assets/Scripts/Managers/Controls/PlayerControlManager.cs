using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerControlManager : MonoBehaviour
{
    static public PlayerControlManager instance;

    static public bool Enabled { get; set; }

    private IPlayerController playerController;

    private bool defending;

    private Enums.ControlType controlType;
    private ExStatusIcon selectionIcon;
    
    public CameraManager cameraManager;

    public List<GameWorldInteractableElementData> potentialTargetList = new List<GameWorldInteractableElementData>();
    public List<GameWorldInteractableElementData> eligibleSelectionTargets = new List<GameWorldInteractableElementData>();

    private GameWorldInteractableElementData SelectionTarget    { get; set; }
    
    private StatusIconOverlay StatusIconOverlay             { get { return cameraManager.overlayManager.StatusIconOverlay; } }

    private PlayerSaveElementData PlayerData                { get { return GameManager.instance.gameSaveData.PlayerSaveData; } }
    private GamePartyMemberElementData ActiveCharacterData  { get { return GameManager.instance.partyMemberData; } }
    private GameElement ActiveCharacter                     { get { return ActiveCharacterData.DataElement.GetComponent<GameElement>(); } }
    private Animator ActiveAnimator                         { get { return ((ExGameWorldAgent)ActiveCharacterData.DataElement.Element).Animator; } }
    
    public Enums.ControlType ControlType
    {
        get { return controlType; }
        set
        {
            controlType = value;
            InitializeControls();
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (potentialTargetList.Count == 0 || TimeManager.instance.Paused) return;

        CheckTargetTriggers();

        if(selectionIcon != null)
        {
            selectionIcon.UpdatePosition();
        }
    }

    private void CheckTargetTriggers()
    {
        eligibleSelectionTargets = new List<GameWorldInteractableElementData>(potentialTargetList);

        //Check if target is faced by the player character
        foreach (var target in potentialTargetList)
        {
            var targetData = (GameWorldInteractableElementData)target.DataElement.ElementData;

            //If the player character must face the interactable
            if (targetData.Interaction.FaceInteractable)
            {
                var directionToTarget = (target.DataElement.transform.position - ActiveCharacter.transform.position).normalized;
                
                if (Vector3.Angle(ActiveCharacter.transform.forward, directionToTarget) < ActiveCharacter.fieldOfView / 2f)
                {
                    RaycastHit hit;

                    //Remove eligible target if nothing is hit by the raycast or if the raycast hit an object other than the target
                    if (!Physics.Raycast(ActiveCharacter.transform.position + ActiveCharacter.transform.up, directionToTarget, out hit, targetData.Interaction.InteractionRange) || 
                        hit.collider.gameObject != target.DataElement.GetComponent<GameElement>().Model.gameObject)
                    {
                        eligibleSelectionTargets.Remove(target);
                        continue;
                    }

                } else {

                    //Remove eligible target if the target is outside of the player character's field of view
                    eligibleSelectionTargets.Remove(target);
                    continue;
                }
            }

            //If the interactable must face the player character
            if (targetData.Interaction.FacePartyLeader)
            {
                var directionToPlayer = (ActiveCharacter.transform.position - target.DataElement.transform.position).normalized;

                if (Vector3.Angle(target.DataElement.transform.forward, directionToPlayer) < target.DataElement.GetComponent<GameElement>().fieldOfView / 2f)
                {
                    RaycastHit hit;

                    //Remove eligible target if nothing is hit by the raycast or if the raycast hit an object other than the player
                    if (!Physics.Raycast(target.DataElement.transform.position + target.DataElement.transform.up, directionToPlayer, out hit, targetData.Interaction.InteractionRange) ||
                        hit.collider.gameObject != ActiveCharacter.Model.gameObject)
                    {
                        eligibleSelectionTargets.Remove(target);
                        continue;
                    }

                } else {

                    //Remove eligible target if the target is outside of the target's field of view
                    eligibleSelectionTargets.Remove(target);
                    continue;
                }
            }

            //If the interactable must be near its final destination
            if (targetData.Interaction.BeNearDestination)
            {
                //Target can only be near it's destination if it's standing still...
                if (targetData.Interaction.ArrivalType == Enums.ArrivalType.Stay)
                {
                    //... at its final destination
                    if (targetData.Interaction.DestinationIndex != targetData.Interaction.InteractionDestinationDataList.Count - 1 ||
                       targetData.AgentState != AgentState.Idle)
                    {
                        eligibleSelectionTargets.Remove(target);
                        continue;
                    }
                }
            }
        }

        //If there are multiple eligible targets, pick the one closest to the player character's forward facing direction
        if(eligibleSelectionTargets.Count > 1)
        {
            var closestTarget = eligibleSelectionTargets.First();

            for(int i = 1; i < eligibleSelectionTargets.Count; i++)
            {
                var incomingDirectionToTarget = (eligibleSelectionTargets[i].DataElement.transform.position - ActiveCharacter.transform.position).normalized;
                var currentDirectionToTarget = (closestTarget.DataElement.transform.position - ActiveCharacter.transform.position).normalized;

                if (Vector3.Angle(ActiveCharacter.transform.forward, incomingDirectionToTarget) < Vector3.Angle(ActiveCharacter.transform.forward, currentDirectionToTarget))
                    closestTarget = eligibleSelectionTargets[i];
            }

            SetSelectionTarget(closestTarget);

        } else {

            SetSelectionTarget(eligibleSelectionTargets.FirstOrDefault());
        }
    }

    private void SetSelectionTarget(GameWorldInteractableElementData target)
    {
        if (SelectionTarget == target) return;
        
        //Cancel current selection
        if (SelectionTarget != null)
        {
            SelectionTarget.DataElement.GetComponent<GameElement>().CancelSelection();
        }

        SelectionTarget = target;

        UpdateControls();

        if (SelectionTarget != null)
        {
            SetSelectionTargetIcon();
            AutoTriggerInteractionEvent();
        }
    }

    private void SetSelectionTargetIcon()
    {
        if (SelectionTarget.Interaction.HideInteractionIndicator) return;

        selectionIcon = StatusIconOverlay.StatusIcon(StatusIconOverlay.StatusIconType.Selection);
        selectionIcon.SetIconTarget(SelectionTarget.DataElement);

        SelectionTarget.DataElement.GetComponent<GameElement>().selectionIcon = selectionIcon.gameObject;
    }
    
    public void RemoveSelectionTarget(GameWorldInteractableElementData gameElement)
    {
        InteractionManager.CancelInteraction(gameElement);

        if (gameElement == SelectionTarget)
        {
            SetSelectionTarget(null);
        }

        potentialTargetList.Remove(gameElement);
    }
    
    public void InitializeControls()
    {
        DestroyImmediate((Object)GetComponent<IPlayerController>());

        switch(ControlType)
        {
            case Enums.ControlType.Touch: playerController = gameObject.AddComponent<TouchControls>(); break;

            default: Debug.Log("CASE MISSING: " + ControlType); break;
        }
    }

    public void UpdateControls()
    {
        switch (ControlType)
        {
            case Enums.ControlType.Touch: UpdateTouchControls(); break;

            default: Debug.Log("CASE MISSING: " + ControlType); break;
        }
    }

    private void UpdateTouchControls()
    {
        var controls = (TouchControls)playerController;
 
        if(SelectionTarget != null)
        {
            controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Primary, Enums.ButtonEventType.Interact);
        } else {
            controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Primary, Enums.ButtonEventType.Attack);
        }
        
        if(InteractionManager.interactionTarget != null && InteractionManager.interactionTarget.Interaction.CancelDelayOnInput)
        {
            controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Secondary, Enums.ButtonEventType.Cancel);
        } else
        {
            controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Secondary, Enums.ButtonEventType.Defend);
        }
    }

    public void AutoTriggerInteractionEvent()
    {
        if (!SelectionTarget.Interaction.TriggerAutomatically) return;

        InteractionEvent();
    }

    public void InteractionEvent()
    {
        InteractionManager.SetInteraction(SelectionTarget);
    }

    public void CancelEvent()
    {
        InteractionManager.CancelInteraction();
    }

    public void AttackEvent()
    {
        Debug.Log("Attack");
    }

    public void DefendEvent()
    {
        defending = !defending;
        
        Debug.Log((defending ? "Start" : "Stop") + " defending");
    }

    public void SetPlayerCharacter()
    {
        Debug.Log("Set character");

        if(ActiveCharacterData.DataElement != null)
        {
            PlayerData.PositionX = ActiveCharacterData.DataElement.transform.localPosition.x;
            PlayerData.PositionY = ActiveCharacterData.DataElement.transform.localPosition.y;
            PlayerData.PositionZ = ActiveCharacterData.DataElement.transform.localPosition.z;
        }
        
        MoveCamera();
    }

    public void MovePlayerCharacter(float sensitivity)
    {
        var speed = ActiveCharacterData.Speed * sensitivity;
        
        ActiveCharacterData.DataElement.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        ActiveAnimator.SetBool("IsMoving", true);
        ActiveAnimator.SetFloat("MoveSpeedSensitivity", speed / ActiveCharacterData.Scale);

        PlayerData.PositionX = ActiveCharacterData.DataElement.transform.localPosition.x;
        PlayerData.PositionY = ActiveCharacterData.DataElement.transform.localPosition.y;
        PlayerData.PositionZ = -ActiveCharacterData.DataElement.transform.localPosition.z;
        
        if(InteractionManager.interactionTarget != null && InteractionManager.interactionTarget.Interaction.CancelDelayOnMovement)
        {
            InteractionManager.CancelInteraction();
        }

        MoveCamera();
    }

    public void RotatePlayerCharacter(float angle)
    {
        ActiveCharacterData.DataElement.transform.eulerAngles = new Vector3(0, -angle, 0);
    }

    private void MoveCamera()
    {
        var cameraDistance = 10;     
        cameraManager.cam.transform.localPosition = new Vector3(PlayerData.PositionX, cameraManager.cam.transform.localPosition.y, -PlayerData.PositionZ - cameraDistance);

        cameraManager.UpdateData();
        cameraManager.overlayManager.GameOverlay.UpdateOverlay();
    }

    public void StopMovingPlayerCharacter()
    {
        ActiveAnimator.SetBool("IsMoving", false);
    }
}
