using UnityEngine;
using System;
using System.Collections.Generic;

public class RegionElementData : RegionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public RegionData OriginalData                  { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Region; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

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

    public void Update()
    {
        if (!Changed) return;

        RegionDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;

        RegionDataManager.UpdateIndex(this);

        OriginalData.Index = Index;
    }

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
        var data = new RegionElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
