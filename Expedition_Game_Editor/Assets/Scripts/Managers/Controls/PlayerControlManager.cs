using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerControlManager : MonoBehaviour
{
    static public PlayerControlManager instance;

    static public bool AllowInput               { get; set; }
    static public bool DisablePlayerMovement    { get; set; }
    static public bool DisableCameraMovement    { get; set; }

    private Vector3 defaultCameraPosition = new Vector3(0, 10, -10);
    private Vector3 defaultCameraRotation = new Vector3(40, 0, 0);

    private IPlayerController playerController;

    private bool defending;

    private Enums.ControlType controlType;
    private ExStatusIcon selectionIcon;

    public List<GameWorldInteractableElementData> potentialTargetList = new List<GameWorldInteractableElementData>();
    public List<GameWorldInteractableElementData> eligibleSelectionTargets = new List<GameWorldInteractableElementData>();

    private GameWorldInteractableElementData SelectionTarget    { get; set; }
    
    private CameraManager CameraManager                         { get { return GameManager.instance.Organizer.CameraManager; } }
    private StatusIconOverlay StatusIconOverlay                 { get { return GameManager.instance.Organizer.OverlayManger.StatusIconOverlay; } }

    private PlayerSaveElementData PlayerData                    { get { return GameManager.instance.gameSaveData.PlayerSaveData; } }
    private GameWorldInteractableElementData ActiveCharacterData{ get { return GameManager.instance.worldInteractableControllableData; } }
    private GameElement ActiveCharacter                         { get { return ActiveCharacterData.DataElement.GetComponent<GameElement>(); } }
    private Animator ActiveAnimator                             { get { return ((ExGameWorldAgent)ActiveCharacterData.DataElement.SelectionElement.Element).Animator; } }
    
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
        
        //Of the potential targets, check which one will be eligible for selection
        CheckTargetTriggers();

        if(selectionIcon != null)
        {
            selectionIcon.UpdatePosition();
        }
    }

    private void CheckTargetTriggers()
    {
        //Don't look for interaction targets if there is an active outcome that can't be cancelled by interacting
        if (InteractionManager.activeOutcome != null && !InteractionManager.activeOutcome.CancelScenarioOnInteraction) return;

        eligibleSelectionTargets = new List<GameWorldInteractableElementData>(potentialTargetList);

        //Check if target is faced by the player character
        foreach (var target in potentialTargetList)
        {
            var targetData = (GameWorldInteractableElementData)target.DataElement.ElementData;

            //Don't target the already active interaction target
            if (InteractionManager.activeOutcome != null && targetData == InteractionManager.interactionTarget)
            {
                eligibleSelectionTargets.Remove(target);
                continue;
            }

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
            if (targetData.Interaction.FaceControllable)
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

    public void SetSelectionTarget(GameWorldInteractableElementData target)
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

        selectionIcon = StatusIconOverlay.StatusIcon(StatusIconOverlay.StatusIconType.Selection, CameraManager.overlayManager.layer[0]);
        selectionIcon.SetIconTarget(SelectionTarget.DataElement);

        SelectionTarget.DataElement.GetComponent<GameElement>().selectionIcon = selectionIcon.gameObject;
    }
    
    public void RemoveSelectionTarget(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        InteractionManager.CancelInteractionDelay(gameWorldInteractableElementData);

        if (gameWorldInteractableElementData == SelectionTarget)
        {
            SetSelectionTarget(null);
        }

        potentialTargetList.Remove(gameWorldInteractableElementData);
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

        UpdatePrimaryButton();
        UpdateSecondaryButton();
    }

    private void UpdatePrimaryButton()
    {
        var controls = (TouchControls)playerController;

        if (SelectionTarget != null || ScenarioManager.allowContinue)
        {
            controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Primary, Enums.ButtonEventType.Interact);
            return;
        }

        controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Primary, Enums.ButtonEventType.Attack);
    }

    private void UpdateSecondaryButton()
    {
        var controls = (TouchControls)playerController;

        if (InteractionManager.interactionDelayTarget != null && InteractionManager.interactionDelayTarget.Interaction.CancelDelayOnInput)
        {
            controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Secondary, Enums.ButtonEventType.Cancel);
            return;
        }

        if (InteractionManager.activeOutcome != null)
        {
            switch((Enums.CancelScenarioType)InteractionManager.activeOutcome.CancelScenarioType)
            {
                case Enums.CancelScenarioType.Cancel:

                    if (InteractionManager.activeOutcome.CancelScenarioOnInput)
                    {
                        controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Secondary, Enums.ButtonEventType.Cancel);
                        return;
                    }

                    break;

                case Enums.CancelScenarioType.Skip:

                    if (InteractionManager.activeOutcome.CancelScenarioType == (int)Enums.CancelScenarioType.Skip)
                    {
                        controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Secondary, Enums.ButtonEventType.Cancel);
                        return;
                    }

                    break;
            } 
        }
        
        controls.TouchOverlay.UpdateTouchButton(Enums.GameButtonType.Secondary, Enums.ButtonEventType.Defend);
    }

    public void AutoTriggerInteractionEvent()
    {
        if (!SelectionTarget.Interaction.TriggerAutomatically) return;

        InteractionEvent();
    }

    public void InteractionEvent()
    {
        if (SelectionTarget != null)
        {
            //Cancel any active interaction before selecting the next
            if (InteractionManager.interactionTarget != null)
                InteractionManager.CancelInteraction();

            InteractionManager.SetInteractionDelay(SelectionTarget);
        }
        
        if (ScenarioManager.allowContinue)
            ScenarioManager.instance.StartNextScene();
    }

    public void CancelEvent()
    {
        if(InteractionManager.interactionDelayTarget != null)
            InteractionManager.CancelInteractionDelay();

        if (InteractionManager.interactionTarget != null)
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
        
        ResetCamera();
    }

    public void SetWorldInteractableControllableTerrainTileId(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        var playerData = GameManager.instance.gameSaveData.PlayerSaveData;
        var regionData = GameManager.instance.gameWorldData.RegionDataList.Where(x => x.Id == playerData.RegionId).First();
        
        gameWorldInteractableElementData.TerrainTileId = RegionManager.GetGameTerrainTileId(regionData, playerData.PositionX, playerData.PositionZ);
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
        
        if(InteractionManager.interactionDelayTarget != null && InteractionManager.interactionDelayTarget.Interaction.CancelDelayOnMovement)
        {
            InteractionManager.CancelInteractionDelay();
        }

        var cameraPosition = new Vector3(PlayerData.PositionX, defaultCameraPosition.y, -PlayerData.PositionZ + defaultCameraPosition.z);

        if (DisableCameraMovement) return;

        MoveCamera(cameraPosition);
    }

    public void RotatePlayerCharacter(float angle)
    {
        ActiveCharacterData.DataElement.transform.eulerAngles = new Vector3(0, -angle, 0);
    }

    public void ResetCamera()
    {
        var cameraPosition = new Vector3(PlayerData.PositionX, defaultCameraPosition.y, -PlayerData.PositionZ + defaultCameraPosition.z);
        CameraManager.cam.transform.eulerAngles = defaultCameraRotation;

        MoveCamera(cameraPosition);
    }

    public void MoveCamera(Vector3 position)
    {
        CameraManager.cam.transform.localPosition = position;

        CameraManager.UpdateData();
        CameraManager.overlayManager.GameOverlay.UpdateOverlay();
    }

    public void StopMovingPlayerCharacter()
    {
        ActiveAnimator.SetBool("IsMoving", false);
    }
}
