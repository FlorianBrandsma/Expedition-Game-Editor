using UnityEngine;
using System;

public class ChapterElementData : ChapterData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ChapterData OriginalData                 { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Chapter; } }

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

    public bool ChangedTimeSpeed
    {
        get { return !Mathf.Approximately(TimeSpeed, OriginalData.TimeSpeed); }
    }

    public bool ChangedEditorNotes
    {
        get { return EditorNotes != OriginalData.EditorNotes; }
    }

    public bool ChangedGameNotes
    {
        get { return PrivateNotes != OriginalData.PrivateNotes; }
    }

    public bool Changed
    {
        get
        {
            return ChangedName || ChangedTimeSpeed || ChangedEditorNotes || ChangedGameNotes;
        }
    }
    #endregion

    public ChapterElementData() { }

    public void Add(DataRequest dataRequest)
    {
        ChapterDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        ChapterDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        ChapterDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        ChapterDataManager.RemoveData(this, dataRequest);  
    }
    
    public void RemoveIndex(DataRequest dataRequest)
    {
        ChapterDataManager.RemoveIndex(this, dataRequest);
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
        var data = new ChapterElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
