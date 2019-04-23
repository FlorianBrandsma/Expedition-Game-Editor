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

    //Each index must represent a pivot from the enum above
    public int[] pivot_position;

    public bool cast_shadow;

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Object;
    }
    #endregion
}
