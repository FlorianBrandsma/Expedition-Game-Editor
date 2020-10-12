using UnityEngine;

public class TouchOverlay : MonoBehaviour, IOverlay
{
    private ExJoystick joystick;
    private ExTouchButton primaryButton;
    private ExTouchButton secondaryButton;
    
    public Texture InteractIcon { get; set; }
    public Texture CancelIcon   { get; set; }
    public Texture AttackIcon   { get; set; }
    public Texture DefendIcon   { get; set; }

    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }

    private void Awake()
    {
        InteractIcon    = Resources.Load<Texture2D>("Textures/Icons/UI/MoreHorizontal");
        CancelIcon      = Resources.Load<Texture2D>("Textures/Icons/UI/Close");
        AttackIcon      = Resources.Load<Texture2D>("Textures/Icons/UI/Polearm");
        DefendIcon      = Resources.Load<Texture2D>("Textures/Icons/UI/Security");
    }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        primaryButton   = TouchButton(Enums.GameButtonType.Primary);
        secondaryButton = TouchButton(Enums.GameButtonType.Secondary);
    }

    public void ActivateOverlay(IOrganizer organizer) { }

    public ExTouchButton TouchButton(Enums.GameButtonType buttonType)
    {
        var prefab = Resources.Load<ExTouchButton>("Elements/UI/TouchButton");
        var touchButton = (ExTouchButton)PoolManager.SpawnObject(prefab);

        touchButton.TouchOverlay = this;

        touchButton.gameObject.SetActive(true);

        touchButton.transform.SetParent(OverlayManager.content, false);

        switch(buttonType)
        {
            case Enums.GameButtonType.Primary:      InitializePrimaryButton(touchButton);     break;
            case Enums.GameButtonType.Secondary:    InitializeSecondaryButton(touchButton);   break;

            default: Debug.Log("CASE MISSING: " + buttonType); break;
        }
        
        return touchButton;
    }
    
    private void InitializePrimaryButton(ExTouchButton touchButton)
    {
        var buttonPosition = new Vector2(-90, 140);
        touchButton.RectTransform.anchoredPosition = buttonPosition;

        touchButton.EventType = Enums.ButtonEventType.Attack;
    }

    private void InitializeSecondaryButton(ExTouchButton touchButton)
    {
        var buttonPosition = new Vector2(-190, 100);
        touchButton.RectTransform.anchoredPosition = buttonPosition;

        touchButton.EventType = Enums.ButtonEventType.Defend;
    }

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

    public void UpdateTouchButton(Enums.GameButtonType buttonType, Enums.ButtonEventType actionType)
    {
        ExTouchButton touchButton = null;

        switch(buttonType)
        {
            case Enums.GameButtonType.Primary:      touchButton = primaryButton;    break;
            case Enums.GameButtonType.Secondary:    touchButton = secondaryButton;  break;

            default: Debug.Log("CASE MISSING: " + buttonType); break;
        }

        touchButton.EventType = actionType;
    }

    public void SetOverlay() { }

    private void CloseTouchButtons()
    {
        PoolManager.ClosePoolObject(primaryButton);
        PoolManager.ClosePoolObject(secondaryButton);
    }

    public void CloseOverlay()
    {
        CloseTouchButtons();

        DestroyImmediate(this);
    }
}
