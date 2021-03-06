﻿using UnityEngine;
using System;

public class TaskElementData : TaskData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public TaskData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Task; } }

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

    public bool ChangedCompleteObjective
    {
        get { return CompleteObjective != OriginalData.CompleteObjective; }
    }

    public bool ChangedRepeatable
    {
        get { return Repeatable != OriginalData.Repeatable; }
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
            return ChangedName || ChangedCompleteObjective || ChangedRepeatable || ChangedEditorNotes || ChangedGameNotes;
        }
    }
    #endregion

    public TaskElementData() { }

    public void Add(DataRequest dataRequest)
    {
        TaskDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        TaskDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        TaskDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        TaskDataManager.RemoveData(this, dataRequest);
    }

    public void RemoveIndex(DataRequest dataRequest)
    {
        TaskDataManager.RemoveIndex(this, dataRequest);
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
        var data = new TaskElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
