using UnityEngine;
using System.Linq;

public class TerrainCore : GeneralData
{
    private int regionId;
    private int iconId;
    private string name;

    public int originalIndex;
    public int originalRegionId;
    public int originalIconId;
    public string originalName;

    private bool changedIndex;
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

    public int Index
    {
        get { return index; }
        set
        {
            if (value == index) return;

            changedIndex = true;

            index = value;
        }
    }

    public int RegionId
    {
        get { return regionId; }
        set
        {
            if (value == regionId) return;

            changedRegionId = (value != originalRegionId);
        }
    }

    public int IconId
    {
        get { return iconId; }
        set
        {
            if (value == iconId) return;

            changedIconId = (value != originalIconId);
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

    public void Create()
    {

    }

    public virtual void Update()
    {
        var terrainData = Fixtures.terrainList.Where(x => x.id == id).FirstOrDefault();

        if (changedIconId)
            terrainData.iconId = iconId;
        
        if (changedName)
            terrainData.name = name;
    }

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

    public void Delete()
    {

    }

    #endregion
}
