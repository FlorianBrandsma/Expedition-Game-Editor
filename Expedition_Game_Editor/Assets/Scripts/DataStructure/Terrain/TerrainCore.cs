using UnityEngine;
using System.Collections;

public class TerrainCore : GeneralData
{
    public bool changed;

    #region Properties

    #endregion

    #region Methods

    public void Create()
    {

    }

    public void Update()
    {
        if (!changed) return;

        SetOriginalValues();
    }

    public void UpdateIndex()
    {

    }

    public void SetOriginalValues()
    {
        ClearChanges();
    }

    public void GetOriginalValues()
    {

    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
    }

    public void Delete()
    {

    }

    #endregion
}
