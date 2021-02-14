using UnityEngine;
using System;

public class ChapterSaveElementData : ChapterSaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ChapterSaveData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.ChapterSave; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedComplete
    {
        get { return Complete != OriginalData.Complete; }
    }

    public bool Changed
    {
        get
        {
            return ChangedComplete;
        }
    }
    #endregion

    public ChapterSaveElementData() { }
    
    public void Add(DataRequest dataRequest)
    {
        ChapterSaveDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        ChapterSaveDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        ChapterSaveDataManager.RemoveData(this, dataRequest);
    }

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
        var data = new ChapterSaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
