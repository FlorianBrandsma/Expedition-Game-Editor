using UnityEngine;
using System.Collections;

public class RegionProperties : MonoBehaviour, IProperties
{
    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Region;
    }
    #endregion
}
