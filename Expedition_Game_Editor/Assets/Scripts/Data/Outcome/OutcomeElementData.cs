using UnityEngine;
using System;

public class OutcomeElementData : OutcomeData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public OutcomeData OriginalData                 { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Outcome; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

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

    public bool ChangedCancelScenarioType
    {
        get { return CancelScenarioType != OriginalData.CancelScenarioType; }
    }

    public bool ChangedCancelScenarioOnInteraction
    {
        get { return CancelScenarioOnInteraction != OriginalData.CancelScenarioOnInteraction; }
    }

    public bool ChangedCancelScenarioOnInput
    {
        get { return CancelScenarioOnInput != OriginalData.CancelScenarioOnInput; }
    }

    public bool ChangedCancelScenarioOnRange
    {
        get { return CancelScenarioOnRange != OriginalData.CancelScenarioOnRange; }
    }

    public bool ChangedCancelScenarioOnHit
    {
        get { return CancelScenarioOnHit != OriginalData.CancelScenarioOnHit; }
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
            return  ChangedCompleteTask                 || ChangedResetObjective        || ChangedCancelScenarioType    ||
                    ChangedCancelScenarioOnInteraction  || ChangedCancelScenarioOnInput || ChangedCancelScenarioOnRange || ChangedCancelScenarioOnHit ||
                    ChangedPublicNotes                  || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Add(DataRequest dataRequest)
    {
        OutcomeDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        OutcomeDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        OutcomeDataManager.RemoveData(this, dataRequest);
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
        var data = new OutcomeElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
