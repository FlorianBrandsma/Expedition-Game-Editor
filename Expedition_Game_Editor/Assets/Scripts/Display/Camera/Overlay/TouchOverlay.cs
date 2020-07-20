using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchOverlay : MonoBehaviour, IOverlay
{
    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }

    private ExJoystick joystick;
    
    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer) { }

    public void InitializeJoystick(Vector2 joystickPosition, float joystickSize)
    {
        var prefab = Resources.Load<ExJoystick>("Elements/UI/Joystick");
        joystick = (ExJoystick)PoolManager.SpawnObject(prefab);

        joystick.gameObject.SetActive(true);

        joystick.transform.SetParent(OverlayManager.content, false);

        joystick.transform.localScale = new Vector3(joystickSize, joystickSize, 1);
        joystick.transform.position = joystickPosition;
    }

    public void DeactivateJoystick()
    {
        if (joystick != null)
            PoolManager.ClosePoolObject(joystick);
    }

    public void RotateJoystick(Vector3 screenPoint, float angle)
    {
        joystick.Rotate(screenPoint, angle);
    }

    public void UpdateOverlay() { }

    public void SetOverlay() { }

    public void CloseOverlay()
    {
        DestroyImmediate(this);
    }
}
