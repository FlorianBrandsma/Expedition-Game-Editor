using UnityEngine;
using System;

public class ObjectiveElementData : ObjectiveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ObjectiveData OriginalData               { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Objective; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedIndex
    {
        get { return Index != OriginalData.Index; }
    }

    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }

    public bool ChangedJournal
    {
        get { return Journal != OriginalData.Journal; }
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
            return ChangedName || ChangedJournal || ChangedEditorNotes || ChangedGameNotes;
        }
    }
    #endregion

    public ObjectiveElementData() { }

    public void Add(DataRequest dataRequest)
    {
        ObjectiveDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        ObjectiveDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        ObjectiveDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        ObjectiveDataManager.RemoveData(this, dataRequest);
    }

    public void RemoveIndex(DataRequest dataRequest)
    {
        ObjectiveDataManager.RemoveIndex(this, dataRequest);
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
        var data = new ObjectiveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
