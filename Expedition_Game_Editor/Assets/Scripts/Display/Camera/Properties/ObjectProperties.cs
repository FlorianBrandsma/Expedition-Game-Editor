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
    public int[] pivotPosition;

    public bool castShadow;

    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Object;
    }

    public SelectionManager.Property SelectionProperty
    {
        get { return SelectionManager.Property.None; }
        set { }
    }

    public SelectionManager.Type SelectionType
    {
        get { return SelectionManager.Type.None; }
        set { }
    }
    #endregion
}
