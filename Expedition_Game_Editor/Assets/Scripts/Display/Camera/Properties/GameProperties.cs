using UnityEngine;
using System.Collections;

public class GameProperties : MonoBehaviour, IProperties
{
    #region IProperties
    public DisplayManager.OrganizerType OrganizerType()
    {
        return DisplayManager.OrganizerType.Game;
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
