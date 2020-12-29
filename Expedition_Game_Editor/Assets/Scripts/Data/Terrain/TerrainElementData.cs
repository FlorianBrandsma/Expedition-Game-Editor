using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementData : TerrainData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public TerrainData OriginalData                 { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Terrain; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public List<AtmosphereElementData> AtmosphereDataList                           { get; set; } = new List<AtmosphereElementData>();
    public List<TerrainTileElementData> TerrainTileDataList                         { get; set; } = new List<TerrainTileElementData>();
    public List<WorldInteractableElementData> WorldInteractableDataList             { get; set; } = new List<WorldInteractableElementData>();
    public List<InteractionDestinationElementData> InteractionDestinationDataList   { get; set; } = new List<InteractionDestinationElementData>();
    public List<WorldObjectElementData> WorldObjectDataList                         { get; set; } = new List<WorldObjectElementData>();
    public List<SceneActorElementData> SceneActorDataList                           { get; set; } = new List<SceneActorElementData>();
    public List<ScenePropElementData> ScenePropDataList                             { get; set; } = new List<ScenePropElementData>();

    #region Changed
    public bool ChangedIconId
    {
        get { return IconId != OriginalData.IconId; }
    }

    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }

    public bool Changed
    {
        get
        {
            return ChangedIconId || ChangedName;
        }
    }
    #endregion

    public void Add(DataRequest dataRequest)
    {
        TerrainDataManager.AddData(this, dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            SetOriginalValues();
    }

    public void Update(DataRequest dataRequest)
    {
        if (!Changed) return;

        TerrainDataManager.UpdateData(this, dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            SetOriginalValues();
    }

    public void UpdateSearch() { }

    public void Remove(DataRequest dataRequest) { }

    public void SetOriginalValues()
    {
        AtmosphereDataList.ForEach(x => x.SetOriginalValues());
        TerrainTileDataList.ForEach(x => x.SetOriginalValues());
        WorldInteractableDataList.ForEach(x => x.SetOriginalValues());
        InteractionDestinationDataList.ForEach(x => x.SetOriginalValues());
        WorldObjectDataList.ForEach(x => x.SetOriginalValues());
        SceneActorDataList.ForEach(x => x.SetOriginalValues());
        ScenePropDataList.ForEach(x => x.SetOriginalValues());

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
        var data = new TerrainElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();
        
        data.AtmosphereDataList             = new List<AtmosphereElementData>(AtmosphereDataList.Select(x => (AtmosphereElementData)x.Clone()));
        data.TerrainTileDataList            = new List<TerrainTileElementData>(TerrainTileDataList.Select(x => (TerrainTileElementData)x.Clone()));
        data.WorldInteractableDataList      = new List<WorldInteractableElementData>(WorldInteractableDataList.Select(x => (WorldInteractableElementData)x.Clone()));
        data.InteractionDestinationDataList = new List<InteractionDestinationElementData>(InteractionDestinationDataList.Select(x => (InteractionDestinationElementData)x.Clone()));
        data.WorldObjectDataList            = new List<WorldObjectElementData>(WorldObjectDataList.Select(x => (WorldObjectElementData)x.Clone()));
        data.SceneActorDataList             = new List<SceneActorElementData>(SceneActorDataList.Select(x => (SceneActorElementData)x.Clone()));
        data.ScenePropDataList              = new List<ScenePropElementData>(ScenePropDataList.Select(x => (ScenePropElementData)x.Clone()));

        base.Clone(data);

        return data;
    }
}