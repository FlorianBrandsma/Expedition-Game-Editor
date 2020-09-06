using UnityEngine;
using System;

public class TerrainTileElementData : TerrainTileData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public TerrainTileData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.TerrainTile; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedTileId
    {
        get { return TileId != OriginalData.TileId; }
    }

    public bool Changed
    {
        get
        {
            return ChangedTileId;
        }
    }
    #endregion

    public void Update() { }

    public void UpdateSearch() { }

    public void SetOriginalValues()
    {
        OriginalData = base.Clone();

        ClearChanges();
    }

    public void ClearChanges()
    {
        if (!Changed) return;

        GetOriginalValues();
    }

    public void GetOriginalValues()
    {
        base.GetOriginalValues(OriginalData);
    }

    public new IElementData Clone()
    {
        var data = new TerrainTileElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
