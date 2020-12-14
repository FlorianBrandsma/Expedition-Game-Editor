using UnityEngine;

public class TouchOverlay : MonoBehaviour, IOverlay
{
    private ExJoystick joystick;
    private ExTouchButton primaryButton;
    private ExTouchButton secondaryButton;

    private int overlayUILayerIndex = 2;

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

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer) { }

    private ExTouchButton TouchButton(Enums.GameButtonType buttonType)
    {
        ExTouchButton touchButton = null;

        switch(buttonType)
        {
            case Enums.GameButtonType.Primary:      touchButton = PrimaryTouchButton();     break;
            case Enums.GameButtonType.Secondary:    touchButton = SecondaryTouchButton();   break;

            default: Debug.Log("CASE MISSING: " + buttonType); break;
        }

        return touchButton;
    }

    private ExTouchButton PrimaryTouchButton()
    {
        if(primaryButton == null)
        {
            primaryButton = SpawnTouchButton();
            InitializePrimaryButton();
        }

        return primaryButton;
    }

    private ExTouchButton SecondaryTouchButton()
    {
        if (secondaryButton == null)
        {
            secondaryButton = SpawnTouchButton();
            InitializeSecondaryButton();
        }

        return secondaryButton;
    }

    private ExTouchButton SpawnTouchButton()
    {
        var prefab = Resources.Load<ExTouchButton>("Elements/UI/TouchButton");
        var touchButton = (ExTouchButton)PoolManager.SpawnObject(prefab);

        touchButton.gameObject.SetActive(true);

        touchButton.transform.SetParent(OverlayManager.layer[overlayUILayerIndex], false);

        touchButton.TouchOverlay = this;
        
        return touchButton;
    }
    
    private void InitializePrimaryButton()
    {
        var buttonPosition = new Vector2(-90, 140);
        primaryButton.RectTransform.anchoredPosition = buttonPosition;

        primaryButton.EventType = Enums.ButtonEventType.Attack;
    }

    private void InitializeSecondaryButton()
    {
        var buttonPosition = new Vector2(-190, 100);
        secondaryButton.RectTransform.anchoredPosition = buttonPosition;

        secondaryButton.EventType = Enums.ButtonEventType.Defend;
    }

    public void InitializeJoystick(Vector2 joystickPosition, float joystickSize)
    {
        var prefab = Resources.Load<ExJoystick>("Elements/UI/Joystick");
        joystick = (ExJoystick)PoolManager.SpawnObject(prefab);
        
        joystick.gameObject.SetActive(true);

        joystick.transform.SetParent(OverlayManager.layer[overlayUILayerIndex], false);

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
        if(actionType == Enums.ButtonEventType.None)
        {
            CloseTouchButton(buttonType);
            return;
        }

        var touchButton = TouchButton(buttonType);
        
        touchButton.EventType = actionType;
    }

    public void SetOverlay() { }

    private void CloseTouchButton(Enums.GameButtonType buttonType)
    {
        switch(buttonType)
        {
            case Enums.GameButtonType.Primary:      ClosePrimaryButton();   break;
            case Enums.GameButtonType.Secondary:    CloseSecondaryButton(); break;

            default: Debug.Log("CASE MISSING: " + buttonType); break;
        }
    }

    private void ClosePrimaryButton()
    {
        if (primaryButton == null) return;

        PoolManager.ClosePoolObject(primaryButton);
        primaryButton = null;
    }

    private void CloseSecondaryButton()
    {
        if (secondaryButton == null) return;

        PoolManager.ClosePoolObject(secondaryButton);
        secondaryButton = null;
    }

    public void CloseOverlay()
    {
        CloseTouchButton(Enums.GameButtonType.Primary);
        CloseTouchButton(Enums.GameButtonType.Secondary);

        DestroyImmediate(this);
    }
}
