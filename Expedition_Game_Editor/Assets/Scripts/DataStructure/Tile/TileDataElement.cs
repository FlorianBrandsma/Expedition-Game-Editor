using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileDataElement : TileCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TileDataElement() : base()
    {
        DataType = Enums.DataType.Tile;
    }

    public string icon;

    public string originalIcon;

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
        var dataElement = new TileDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var tileDataSource = (TileDataElement)dataSource;

        icon = tileDataSource.icon;

        SetOriginalValues();
    }
}
