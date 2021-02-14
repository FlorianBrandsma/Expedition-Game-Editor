using UnityEngine;
using System;

public class QuestElementData : QuestData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public QuestData OriginalData                   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Quest; } }

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
            return ChangedName || ChangedEditorNotes || ChangedGameNotes;
        }
    }
    #endregion

    public QuestElementData() { }

    public void Add(DataRequest dataRequest)
    {
        QuestDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        QuestDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        QuestDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        QuestDataManager.RemoveData(this, dataRequest);
    }

    public void RemoveIndex(DataRequest dataRequest)
    {
        QuestDataManager.RemoveIndex(this, dataRequest);
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
        var data = new QuestElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}