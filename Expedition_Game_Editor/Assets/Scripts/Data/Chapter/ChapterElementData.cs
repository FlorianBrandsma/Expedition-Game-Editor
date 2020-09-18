using UnityEngine;
using System;

public class ChapterElementData : ChapterData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ChapterData OriginalData                 { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Chapter; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

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

    public bool ChangedPublicNotes
    {
        get { return PublicNotes != OriginalData.PublicNotes; }
    }

    public bool ChangedPrivateNotes
    {
        get { return PrivateNotes != OriginalData.PrivateNotes; }
    }

    public bool Changed
    {
        get
        {
            return ChangedName || ChangedTimeSpeed || ChangedPublicNotes || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        ChapterDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;

        ChapterDataManager.UpdateIndex(this);

        OriginalData.Index = Index;
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
        var data = new ChapterElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
