using UnityEngine;
using System.Collections;

public class WorldProperties : MonoBehaviour, IProperties
{
    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.World;
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
