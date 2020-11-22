using UnityEngine;
using System;

public class ChapterRegionElementData : ChapterRegionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ChapterRegionData OriginalData           { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.ChapterRegion; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedRegionId
    {
        get { return RegionId != OriginalData.RegionId; }
    }

    public bool Changed
    {
        get
        {
            return ChangedRegionId;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        ChapterRegionDataManager.UpdateData(this);

        SetOriginalValues();
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
        var data = new ChapterRegionElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();
        
        base.Clone(data);

        return data;
    }
}