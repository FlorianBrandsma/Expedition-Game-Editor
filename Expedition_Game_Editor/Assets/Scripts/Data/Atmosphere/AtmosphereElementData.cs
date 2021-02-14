using UnityEngine;
using System;

public class AtmosphereElementData : AtmosphereData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public AtmosphereData OriginalData              { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Atmosphere; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedStartTime
    {
        get { return StartTime != OriginalData.StartTime; }
    }

    public bool ChangedEndTime
    {
        get { return EndTime != OriginalData.EndTime; }
    }

    public bool ChangedEditorNotes
    {
        get { return EditorNotes != OriginalData.EditorNotes; }
    }

    public bool ChangedGameNotes
    {
        get { return GameNotes != OriginalData.GameNotes; }
    }

    public bool Changed
    {
        get
        {
            return ChangedStartTime || ChangedEndTime || ChangedEditorNotes || ChangedGameNotes;
        }
    }
    #endregion

    public AtmosphereElementData() { }

    public void Add(DataRequest dataRequest)
    {
        AtmosphereDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        AtmosphereDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        AtmosphereDataManager.RemoveData(this, dataRequest);
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
        var data = new AtmosphereElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
