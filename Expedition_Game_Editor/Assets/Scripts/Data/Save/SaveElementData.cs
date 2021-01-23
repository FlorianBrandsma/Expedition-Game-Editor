using UnityEngine;
using System;

public class SaveElementData : SaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public SaveData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Save; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }
    
    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public SaveElementData() { }

    public void Add(DataRequest dataRequest)
    {
        SaveDataManager.AddData(this, dataRequest);
    }

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
        var data = new SaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
