using UnityEngine;
using System.Collections;

public class SceneProperties : MonoBehaviour, IProperties
{
    #region IProperties
    public DisplayManager.Type Type()
    {
        return DisplayManager.Type.Scene;
    }
    #endregion
}
