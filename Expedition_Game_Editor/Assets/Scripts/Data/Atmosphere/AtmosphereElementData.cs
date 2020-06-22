using UnityEngine;

public class AtmosphereElementData : AtmosphereCore, IElementData
{
    public DataElement DataElement { get; set; }

    public AtmosphereElementData() : base()
    {
        DataType = Enums.DataType.Atmosphere;
    }
    
    public string regionName;
    public string terrainName;

    public string iconPath;
    public string baseTilePath;

    public bool timeConflict;
    public bool containsActiveTime;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new AtmosphereElementData();

        Debug.Log("Probably remove this");
        elementData.DataElement = DataElement;

        elementData.regionName = regionName;

        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var atmosphereDataSource = (AtmosphereElementData)dataSource;

        timeConflict = atmosphereDataSource.timeConflict;

        regionName = atmosphereDataSource.regionName;
        terrainName = atmosphereDataSource.terrainName;

        iconPath = atmosphereDataSource.iconPath;
        baseTilePath = atmosphereDataSource.baseTilePath;

        SetOriginalValues();
    }
}
