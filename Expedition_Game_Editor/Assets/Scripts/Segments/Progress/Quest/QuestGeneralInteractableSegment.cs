using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestGeneralInteractableSegment : MonoBehaviour, ISegment
{
    private QuestEditor QuestEditor { get { return (QuestEditor)DataEditor; } }

    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchParameters = new Search.PhaseInteractable();

        searchParameters.requestType = Search.PhaseInteractable.RequestType.Custom;
        searchParameters.phaseId = new List<int>() { QuestEditor.QuestData.PhaseId };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });

        var questInteractableList = SegmentController.DataController.DataList.Cast<PhaseInteractableDataElement>().ToList();
        questInteractableList.ForEach(x => QuestEditor.questInteractableDataList.Add(x));
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
