using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class ButtonProperties : MonoBehaviour, IProperties
{
    public void Copy(ButtonProperties new_properties)
    {

    }

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Button;
    }
    #endregion
}
