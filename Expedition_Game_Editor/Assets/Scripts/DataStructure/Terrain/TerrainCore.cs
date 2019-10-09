﻿using UnityEngine;
using System.Linq;

public class TerrainCore : GeneralData
{
    private int regionId;
    private int iconId;
    private string name;

    public int originalRegionId;
    public int originalIconId;
    public string originalName;

    private bool changedRegionId;
    private bool changedIconId;
    private bool changedName;

    public bool Changed
    {
        get
        {
            return changedIconId || changedName;
        }
    }

    #region Properties

    public int RegionId
    {
        get { return regionId; }
        set
        {
            if (value == regionId) return;

            changedRegionId = (value != originalRegionId);

            regionId = value;
        }
    }

    public int IconId
    {
        get { return iconId; }
        set
        {
            if (value == iconId) return;
            
            changedIconId = (value != originalIconId);

            iconId = value;
        }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);
            
            name = value;
        }
    }

    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update()
    {
        var terrainData = Fixtures.terrainList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedIconId)
            terrainData.iconId = iconId;

        if (changedName)
            terrainData.name = name;
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalName = Name;
        originalIconId = IconId;
    }

    public void GetOriginalValues()
    {
        Name = originalName;
        IconId = originalIconId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedIconId = false;
        changedName = false;
    }

    public void Delete() { }

    #endregion
}
