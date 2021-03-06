﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class RegionElementData : RegionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public RegionData OriginalData                  { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Region; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public List<TerrainElementData> TerrainDataList { get; set; } = new List<TerrainElementData>();

    #region Changed
    public bool ChangedChapterRegionId
    {
        get { return ChapterRegionId != OriginalData.ChapterRegionId; }
    }

    public bool ChangedPhaseId
    {
        get { return PhaseId != OriginalData.PhaseId; }
    }

    public bool ChangedTileSetId
    {
        get { return TileSetId != OriginalData.TileSetId; }
    }

    public bool ChangedIndex
    {
        get { return Index != OriginalData.Index; }
    }

    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }

    public bool ChangedRegionSize
    {
        get { return RegionSize != OriginalData.RegionSize; }
    }

    public bool ChangedTerrainSize
    {
        get { return TerrainSize != OriginalData.TerrainSize; }
    }

    public bool Changed
    {
        get
        {
            return ChangedChapterRegionId || ChangedPhaseId || ChangedTileSetId || ChangedName || ChangedRegionSize || ChangedTerrainSize;
        }
    }
    #endregion

    public RegionElementData() { }

    public void Add(DataRequest dataRequest)
    {
        RegionDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        RegionDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        RegionDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        RegionDataManager.RemoveData(this, dataRequest);
    }

    public void RemoveIndex(DataRequest dataRequest)
    {
        RegionDataManager.RemoveIndex(this, dataRequest);
    }

    public void SetOriginalValues()
    {
        TerrainDataList.ForEach(x => x.SetOriginalValues());

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
        TerrainDataList.ForEach(x => x.GetOriginalValues());

        base.GetOriginalValues(OriginalData);
    }

    public new IElementData Clone()
    {
        var data = new RegionElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        data.TerrainDataList = new List<TerrainElementData>(TerrainDataList.Select(x => (TerrainElementData)x.Clone()).ToList());

        base.Clone(data);

        return data;
    }
}
