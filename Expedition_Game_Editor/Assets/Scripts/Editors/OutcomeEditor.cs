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

    public bool Loaded { get; set; }

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

    public string PublicNotes
    {
        get { return outcomeData.PublicNotes; }
        set
        {
            outcomeData.PublicNotes = value;

            DataList.ForEach(x => ((OutcomeElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return outcomeData.PrivateNotes; }
        set
        {
            outcomeData.PrivateNotes = value;

            DataList.ForEach(x => ((OutcomeElementData)x).PrivateNotes = value);
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

    public void OpenEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
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
                UpdateEditor();
                break;
        }
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x =>
        {
            x.ExecuteType = Enums.ExecuteType.Update;
            x.ClearChanges();
        });

        Loaded = false;
    }

    public void CloseEditor() { }
}