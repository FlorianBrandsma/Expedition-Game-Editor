using UnityEngine;
using System;
using System.Collections.Generic;

public class GameWorldElementData : GameWorldData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameWorldData OriginalData               { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameWorld; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public List<GameRegionElementData> RegionDataList { get; set; } = new List<GameRegionElementData>();

    //[Might have to go in terrain to preserve the possibility to generate terrains]
    //There's actually no real way to know what terrain a world interactable belongs to
    //Generated interactables/interactions can be bound to a terrain by id and removed from the list when necessary
    public List<GameWorldInteractableElementData> WorldInteractableDataList { get; set; } = new List<GameWorldInteractableElementData>();

    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public GameWorldElementData() { }

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest) { }

    public void Remove(DataRequest dataRequest) { }

    public void SetOriginalValues()
    {
        OriginalData = base.Clone();

        WorldInteractableDataList.ForEach(x => x.SetOriginalValues());

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
        var data = new GameWorldElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
