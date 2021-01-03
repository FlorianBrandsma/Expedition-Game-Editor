using UnityEngine;
using System;

public class GameRegionElementData : GameRegionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameRegionData OriginalData              { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameRegion; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }
    
    public float Size
    {
        get { return RegionSize * TerrainSize * TileSize; }
    }

    #region ElementData
    public bool Changed { get { return false; } }
    #endregion

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest) { }

    public void Remove(DataRequest dataRequest) { }

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
        var data = new GameRegionElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
