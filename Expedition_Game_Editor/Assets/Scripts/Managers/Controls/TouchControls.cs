﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControls : MonoBehaviour, IPlayerController
{
    public TouchOverlay TouchOverlay { get { return GameManager.instance.Organizer.OverlayManger.TouchOverlay; } }
    
    private float minSensitivity = 0.1f;

    private Vector2 mouseClickPos;
    private Vector3 joystickPosition;
    private Vector2 sensitivity;

    private float joystickOffset = 1.5f;
    private float joystickSize = 1.5f;

    private bool isPointerOverGameObject;

    void Update()
    {
        if (!PlayerControlManager.AllowInput || TimeManager.instance.Paused)
        {
            //CancelInput();
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (isPointerOverGameObject = IsPointerOverGameObject()) return;

            InitializeInput();
        }

        if (Input.GetMouseButton(0))
        {
            if (isPointerOverGameObject) return;

            MoveInput();     
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelInput();
        }
    }

    private bool IsPointerOverGameObject()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }

    private void InitializeInput()
    {
        mouseClickPos.x = Input.mousePosition.x;
        mouseClickPos.y = Input.mousePosition.y;

        joystickPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                       Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        TouchOverlay.InitializeJoystick(joystickPosition, joystickSize);
    }
    
    private void MoveInput()
    {
        if (PlayerControlManager.DisablePlayerMovement) return;

        sensitivity.x = Mathf.Clamp(((Input.mousePosition.x - mouseClickPos.x) / (joystickSize * 100)) * joystickOffset, -1, 1);
        sensitivity.y = Mathf.Clamp(((Input.mousePosition.y - mouseClickPos.y) / (joystickSize * 100)) * joystickOffset, -1, 1);

        TouchOverlay.UpdateOverlay();

        if (sensitivity.x > minSensitivity || sensitivity.y > minSensitivity || sensitivity.x < -minSensitivity || sensitivity.y < -minSensitivity)
        {
            Rotate();
            PlayerControlManager.instance.MovePlayerCharacter(Mathf.Abs(sensitivity.x) > Mathf.Abs(sensitivity.y) ? Mathf.Abs(sensitivity.x) : Mathf.Abs(sensitivity.y));

        } else {

            PlayerControlManager.instance.StopMovingPlayerCharacter();
        }
    }

    private void Rotate()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(joystickPosition);
        var angle = Mathf.Atan2(Input.mousePosition.x - screenPoint.x, Input.mousePosition.y - screenPoint.y) / (2 * Mathf.PI) * 360;

        TouchOverlay.RotateJoystick(screenPoint, angle);

        PlayerControlManager.instance.RotatePlayerCharacter(-angle);
    }

    private void CancelInput()
    {
        if(TouchOverlay != null)
            TouchOverlay.DeactivateJoystick();

        PlayerControlManager.instance.StopMovingPlayerCharacter();
    }
}
