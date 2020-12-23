using UnityEngine;
using UnityEngine.UI;

public class ExTouchButton : MonoBehaviour, IPoolable
{
    private Enums.ButtonEventType eventType;

    public RawImage icon;
    
    public TouchOverlay TouchOverlay        { get; set; }

    public Button Button                    { get { return GetComponent<Button>(); } }
    public RectTransform RectTransform      { get { return GetComponent<RectTransform>(); } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.TouchButton; } }
    public int PoolId                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public Enums.ButtonEventType EventType
    {
        get { return eventType; }
        set
        {
            eventType = value;
            
            switch(eventType)
            {
                case Enums.ButtonEventType.Interact: icon.texture = TouchOverlay.InteractIcon;   break;
                case Enums.ButtonEventType.Cancel:   icon.texture = TouchOverlay.CancelIcon;     break;
                case Enums.ButtonEventType.Attack:   icon.texture = TouchOverlay.AttackIcon;     break;
                case Enums.ButtonEventType.Defend:   icon.texture = TouchOverlay.DefendIcon;     break;

                default: Debug.Log("CASE MISSING: " + eventType); break;
            }
        }
    }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }
    
    public void PointUpEvent()
    {
        if (TimeManager.instance.Paused) return;

        var playerControlManager = PlayerControlManager.instance;

        switch (EventType)
        {
            case Enums.ButtonEventType.Interact: playerControlManager.InteractionEvent();   break;
            case Enums.ButtonEventType.Cancel:   playerControlManager.CancelEvent();        break;
            case Enums.ButtonEventType.Attack:   playerControlManager.AttackEvent();        break;
            case Enums.ButtonEventType.Defend:   playerControlManager.DefendEvent();        break;

            default: Debug.Log("CASE MISSING: " + eventType); break;
        }
    }

    public void PointDownEvent()
    {
        if (TimeManager.instance.Paused) return;

        var playerControlManager = PlayerControlManager.instance;

        switch (EventType)
        {
            case Enums.ButtonEventType.Interact:    break;
            case Enums.ButtonEventType.Cancel:      break;
            case Enums.ButtonEventType.Attack:      break;
            case Enums.ButtonEventType.Defend: playerControlManager.DefendEvent(); break;

            default: Debug.Log("CASE MISSING: " + eventType); break;
        }
    }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
