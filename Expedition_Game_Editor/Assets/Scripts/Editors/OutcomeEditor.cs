using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OutcomeEditor : MonoBehaviour, IEditor
{
    private OutcomeData outcomeData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == outcomeData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded                              { get; set; }

    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return outcomeData.Id; }
    }

    public bool CompleteTask
    {
        get { return outcomeData.CompleteTask; }
        set
        {
            outcomeData.CompleteTask = value;

            DataList.ForEach(x => ((OutcomeElementData)x).CompleteTask = value);
        }
    }

    public bool ResetObjective
    {
        get { return outcomeData.ResetObjective; }
        set
        {
            outcomeData.ResetObjective = value;

            DataList.ForEach(x => ((OutcomeElementData)x).ResetObjective = value);
        }
    }

    public int CancelScenarioType
    {
        get { return outcomeData.CancelScenarioType; }
        set
        {
            outcomeData.CancelScenarioType = value;

            DataList.ForEach(x => ((OutcomeElementData)x).CancelScenarioType = value);
        }
    }

    public bool CancelScenarioOnInteraction
    {
        get { return outcomeData.CancelScenarioOnInteraction; }
        set
        {
            outcomeData.CancelScenarioOnInteraction = value;

            DataList.ForEach(x => ((OutcomeElementData)x).CancelScenarioOnInteraction = value);
        }
    }

    public bool CancelScenarioOnInput
    {
        get { return outcomeData.CancelScenarioOnInput; }
        set
        {
            outcomeData.CancelScenarioOnInput = value;

            DataList.ForEach(x => ((OutcomeElementData)x).CancelScenarioOnInput = value);
        }
    }

    public bool CancelScenarioOnRange
    {
        get { return outcomeData.CancelScenarioOnRange; }
        set
        {
            outcomeData.CancelScenarioOnRange = value;

            DataList.ForEach(x => ((OutcomeElementData)x).CancelScenarioOnRange = value);
        }
    }

    public bool CancelScenarioOnHit
    {
        get { return outcomeData.CancelScenarioOnHit; }
        set
        {
            outcomeData.CancelScenarioOnHit = value;

            DataList.ForEach(x => ((OutcomeElementData)x).CancelScenarioOnHit = value);
        }
    }

    public string EditorNotes
    {
        get { return outcomeData.EditorNotes; }
        set
        {
            outcomeData.EditorNotes = value;

            DataList.ForEach(x => ((OutcomeElementData)x).EditorNotes = value);
        }
    }

    public string GameNotes
    {
        get { return outcomeData.GameNotes; }
        set
        {
            outcomeData.GameNotes = value;

            DataList.ForEach(x => ((OutcomeElementData)x).GameNotes = value);
        }
    }

    public string ModelIconPath
    {
        get { return outcomeData.ModelIconPath; }
    }

    public bool DefaultInteraction
    {
        get { return outcomeData.DefaultInteraction; }
    }

    public int InteractionStartTime
    {
        get { return outcomeData.InteractionStartTime; }
    }

    public int InteractionEndTime
    {
        get { return outcomeData.InteractionEndTime; }
    }

    public string TaskName
    {
        get { return outcomeData.TaskName; }
    }
    #endregion

    public void InitializeEditor()
    {
        outcomeData = (OutcomeData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Addable()
    {
        return true;
    }

    public bool Applicable()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return false;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyOutcomeChanges(dataRequest);
    }

    private void ApplyOutcomeChanges(DataRequest dataRequest)
    {
        if (EditData.ExecuteType == Enums.ExecuteType.Update)
            UpdateOutcome(dataRequest);
    }

    private void UpdateOutcome(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}