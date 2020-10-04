using UnityEngine;
using System;
using System.Collections.Generic;

public class GameTerrainElementData : GameTerrainData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameTerrainData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameTerrain; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public List<GameAtmosphereElementData> AtmosphereDataList               { get; set; } = new List<GameAtmosphereElementData>();
    public List<GameTerrainTileElementData> TerrainTileDataList             { get; set; } = new List<GameTerrainTileElementData>();
    public List<GameWorldInteractableElementData> WorldInteractableDataList { get; set; } = new List<GameWorldInteractableElementData>();
    public List<GameWorldObjectElementData> WorldObjectDataList             { get; set; } = new List<GameWorldObjectElementData>();

    #region Changed
    public bool Changed { get { return false; } }
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
        var data = new GameTerrainElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
