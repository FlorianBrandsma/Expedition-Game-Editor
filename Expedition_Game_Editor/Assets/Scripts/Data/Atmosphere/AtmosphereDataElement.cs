using UnityEngine;
using System.Collections.Generic;

public class AtmosphereDataElement : AtmosphereCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public AtmosphereDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new AtmosphereDataElement();

        Debug.Log("Probably remove this");
        dataElement.DataElement = DataElement;

        dataElement.regionName = regionName;

        CloneCore(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var atmosphereDataSource = (AtmosphereDataElement)dataSource;

        timeConflict = atmosphereDataSource.timeConflict;

        regionName = atmosphereDataSource.regionName;
        terrainName = atmosphereDataSource.terrainName;

        iconPath = atmosphereDataSource.iconPath;
        baseTilePath = atmosphereDataSource.baseTilePath;

        SetOriginalValues();
    }
}
