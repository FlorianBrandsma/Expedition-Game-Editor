using UnityEngine;
using System;

public class ObjectiveSaveElementData : ObjectiveSaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ObjectiveSaveData OriginalData           { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.ObjectiveSave; } }

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

    public ObjectiveSaveElementData() { }

    public void Add(DataRequest dataRequest)
    {
        ObjectiveSaveDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        ObjectiveSaveDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        ObjectiveSaveDataManager.RemoveData(this, dataRequest);
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
        var data = new ObjectiveSaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
