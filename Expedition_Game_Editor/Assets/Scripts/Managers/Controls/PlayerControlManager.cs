using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    static public PlayerControlManager instance;

    private IPlayerController playerController;

    private PlayerSaveElementData PlayerData            { get { return GameManager.instance.gameSaveData.playerSaveData; } }
    private GamePartyMemberElementData ActiveCharacter  { get { return GameManager.instance.partyMemberData; } }
    private Animator ActiveAnimator                     { get { return ((ExGameWorldAgent)ActiveCharacter.DataElement.Element).Animator; } }

    private Enums.ControlType controlType;
    public Enums.ControlType ControlType
    {
        get { return controlType; }
        set
        {
            controlType = value;
            InitializeControls();
        }
    }

    public CameraManager cameraManager;

    public GameWorldInteractableElementData playerCharacter;

    static public bool Enabled { get; set; }

    private void Awake()
    {
        instance = this;
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

    public void SetPlayerCharacter()
    {
        Debug.Log("Set character");

        if(ActiveCharacter.DataElement != null)
        {
            PlayerData.PositionX = ActiveCharacter.DataElement.transform.localPosition.x;
            PlayerData.PositionY = ActiveCharacter.DataElement.transform.localPosition.y;
            PlayerData.PositionZ = ActiveCharacter.DataElement.transform.localPosition.z;
        }
        
        MoveCamera();
    }

    public void MovePlayerCharacter(float sensitivity)
    {
        var speed = ActiveCharacter.speed * sensitivity;
        
        ActiveCharacter.DataElement.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        ActiveAnimator.SetBool("IsMoving", true);
        ActiveAnimator.SetFloat("MoveSpeedSensitivity", speed);

        PlayerData.PositionX = ActiveCharacter.DataElement.transform.localPosition.x;
        PlayerData.PositionY = ActiveCharacter.DataElement.transform.localPosition.y;
        PlayerData.PositionZ = -ActiveCharacter.DataElement.transform.localPosition.z;

        MoveCamera();
    }

    public void RotatePlayerCharacter(float angle)
    {
        ActiveCharacter.DataElement.transform.eulerAngles = new Vector3(0, -angle, 0);
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
