using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    static public PlayerControlManager instance;

    private IPlayerController playerController;

    private PlayerSaveElementData PlayerData { get { return GameManager.instance.gameSaveData.playerSaveData; } }
    private GamePartyMemberElementData ActiveCharacter { get { return GameManager.instance.ActiveCharacter; } }

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

        //animator.SetBool("IsWalking", true);
        //animator.SetFloat("WalkSpeedSensitivity", speed);

        //tempPos = new Vector3(tempPos.x, tempPos.y, tempPos.z - 1 * Time.deltaTime);

        PlayerData.PositionX = ActiveCharacter.DataElement.transform.localPosition.x;
        PlayerData.PositionY = ActiveCharacter.DataElement.transform.localPosition.y;
        PlayerData.PositionZ = -ActiveCharacter.DataElement.transform.localPosition.z;

        MoveCamera();
    }

    public void RotatePlayerCharacter(float angle)
    {
        ActiveCharacter.DataElement.transform.eulerAngles = new Vector3(0, -angle, 0);
        //Debug.Log(angle);
        //Probably wrong angle, but the it's the idea that matters

        //transform.eulerAngles = new Vector3(0, -angle, 0);
    }

    private void MoveCamera()
    {
        //Camera position is based on the saved player data
        //saved player position is updated whenever the player is moving

        var cameraDistance = 10;     
        cameraManager.cam.transform.localPosition = new Vector3(PlayerData.PositionX, cameraManager.cam.transform.localPosition.y, -PlayerData.PositionZ - cameraDistance);

        cameraManager.UpdateData();
    }
}
