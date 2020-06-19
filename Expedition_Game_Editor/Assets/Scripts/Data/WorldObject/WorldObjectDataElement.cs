using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WorldObjectDataElement : WorldObjectCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public WorldObjectDataElement() : base()
    {
        DataType = Enums.DataType.WorldObject;
    }

    public string objectGraphicPath;

    public string objectGraphicName;
    public string objectGraphicIconPath;

    public float height;
    public float width;
    public float depth;

    public string originalObjectGraphicName;
    public string originalObjectGraphicIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void UpdateSearch()
    {
        base.UpdateSearch();
        
        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIconPath = objectGraphicIconPath;
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicName = originalObjectGraphicName;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new WorldObjectDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var worldObjectDataSource = (WorldObjectDataElement)dataSource;
        
        objectGraphicPath = worldObjectDataSource.objectGraphicPath;

        objectGraphicName = worldObjectDataSource.objectGraphicName;
        objectGraphicIconPath = worldObjectDataSource.objectGraphicIconPath;

        height = worldObjectDataSource.height;
        width = worldObjectDataSource.width;
        depth = worldObjectDataSource.depth;
        
        SetOriginalValues();
    }
}
