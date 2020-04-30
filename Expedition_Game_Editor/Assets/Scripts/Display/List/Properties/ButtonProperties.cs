using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class ButtonProperties : MonoBehaviour, IProperties
{
    #region IProperties
    public DisplayManager.OrganizerType OrganizerType()
    {
        return DisplayManager.OrganizerType.Button;
    }
    #endregion
}
