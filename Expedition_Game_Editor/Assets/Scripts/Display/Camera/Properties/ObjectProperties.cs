using UnityEngine;
using System.Collections;

public class ObjectProperties : MonoBehaviour, IProperties
{
    public enum Pivot
    {
        None,
        Top,
        Center,
        Bottom,
        Left,
        Right
    }

    public Pivot pivot;

    public bool display_shadow;

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Object;
    }
    #endregion
}
