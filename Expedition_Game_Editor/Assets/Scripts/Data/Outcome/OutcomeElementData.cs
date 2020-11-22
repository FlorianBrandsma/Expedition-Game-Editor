using UnityEngine;
using System;

public class OutcomeElementData : OutcomeData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public OutcomeData OriginalData                 { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Outcome; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedCompleteTask
    {
        get { return CompleteTask != OriginalData.CompleteTask; }
    }

    public bool ChangedResetObjective
    {
        get { return ResetObjective != OriginalData.ResetObjective; }
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
            return  ChangedCompleteTask ||  ChangedResetObjective ||
                    ChangedPublicNotes  ||  ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        OutcomeDataManager.UpdateData(this);

        SetOriginalValues();
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
        var data = new OutcomeElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
