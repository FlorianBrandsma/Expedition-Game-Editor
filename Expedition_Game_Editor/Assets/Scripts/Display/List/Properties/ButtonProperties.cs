using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class ButtonProperties : MonoBehaviour, IProperties
{
    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Button;
    }
    #endregion
}
