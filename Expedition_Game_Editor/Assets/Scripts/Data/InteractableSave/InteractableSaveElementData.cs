using UnityEngine;
using System;

public class InteractableSaveElementData : InteractableSaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public InteractableSaveData OriginalData        { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.InteractableSave; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }
    
    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public InteractableSaveElementData() { }

    public void Add(DataRequest dataRequest)
    {
        InteractableSaveDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest) { }

    public void Remove(DataRequest dataRequest)
    {
        InteractableSaveDataManager.RemoveData(this, dataRequest);
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
        var data = new InteractableSaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
