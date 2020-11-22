using UnityEngine;
using System;

public class QuestElementData : QuestData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public QuestData OriginalData                   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Quest; } }

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
            return ChangedName || ChangedPublicNotes || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        QuestDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;

        QuestDataManager.UpdateIndex(this);

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
        var data = new QuestElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}