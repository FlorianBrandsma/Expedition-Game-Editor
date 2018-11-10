using UnityEngine;

[RequireComponent(typeof(ListProperties))]

public class ButtonProperties : MonoBehaviour, IProperties
{
    public void Copy(ButtonProperties new_properties)
    {

    }

    #region IProperties
    public ListProperties.Type Type()
    {
        return ListProperties.Type.Button;
    }
    #endregion
}
