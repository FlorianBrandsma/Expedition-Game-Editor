using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour, IPlayerController
{
    private RawImage joystick;
    private RawImage joystickDirection;

    private float minSensitivity = 0.1f;

    private Vector2 mouseClickPos;
    private Vector3 joystickPos;
    private Vector2 sensitivity;

    private float joystickOffset = 1.5f;
    private float joystickSize = 2f;

    private void Awake()
    {
        //joystick = Resources.Load<RawImage>("Textures/Icons/Status/SelectIcon");
        //joystickDirection = Resources.Load<RawImage>("Textures/Icons/Status/LockIcon");
    }

    void Update()
    {
        if (!PlayerControlManager.Enabled)
        {
            DeactivateJoystick();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            InitializeJoystick();
        }

        if (Input.GetMouseButton(0))
        {
            Move();
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            DeactivateJoystick();
            //playerCharacter.StopMoving();
        }
    }

    private void InitializeJoystick()
    {
        mouseClickPos.x = Input.mousePosition.x;
        mouseClickPos.y = Input.mousePosition.y;

        joystickPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                  Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        //joystick.transform.localScale   = new Vector3(joystickSize, joystickSize, 1);
        //joystick.transform.position     = joystickPos

        //joystick.gameObject.SetActive(true);
    }

    private void DeactivateJoystick()
    {
        //joystick.gameObject.SetActive(false);
    }

    private void Move()
    {
        var joystickSize = 200;

        sensitivity.x = Mathf.Clamp(((Input.mousePosition.x - mouseClickPos.x) / joystickSize) * joystickOffset, -1, 1);
        sensitivity.y = Mathf.Clamp(((Input.mousePosition.y - mouseClickPos.y) / joystickSize) * joystickOffset, -1, 1);

        if (sensitivity.x > minSensitivity || sensitivity.y > minSensitivity || sensitivity.x < -minSensitivity || sensitivity.y < -minSensitivity)
        {
            Rotate();
            PlayerControlManager.instance.MovePlayerCharacter(Mathf.Abs(sensitivity.x) > Mathf.Abs(sensitivity.y) ? Mathf.Abs(sensitivity.x) : Mathf.Abs(sensitivity.y));
        }
    }

    private void Rotate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(joystickPos);

        float angle = Mathf.Atan2(Input.mousePosition.x - screenPos.x, Input.mousePosition.y - screenPos.y) / (2 * Mathf.PI) * 360;

        //joystickDirection.transform.eulerAngles = new Vector3(0, 0, -angle);

        float newDist = Mathf.Clamp01(Vector3.Distance(screenPos, Input.mousePosition) / 65);

        //joystickDirection.transform.localScale = new Vector3(0.5f + newDist / 2, 0.5f + newDist / 2, 1);

        PlayerControlManager.instance.RotatePlayerCharacter(-angle);
    }

    private void RotateJoystickDirection()
    {
        
    }

    private void RotateCharacter()
    {
        
        
    }

    //static public void LockControls()
    //{
    //    Enabled = true;
    //}
}
