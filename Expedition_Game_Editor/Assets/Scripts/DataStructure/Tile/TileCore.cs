using UnityEngine;
using System.Collections;

public class TileCore : GeneralData
{
    public bool Changed
    {
        get
        {
            return false;
        }
    }

    #region Properties

    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        //Debug.Log("Update index " + index);
        changedIndex = false;
    }

    public virtual void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public virtual void ClearChanges()
    {
        GetOriginalValues();
    }

    public void Delete() { }

    #endregion
}
