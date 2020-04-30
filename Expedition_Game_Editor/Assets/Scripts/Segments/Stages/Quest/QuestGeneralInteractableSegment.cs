﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestGeneralInteractableSegment : MonoBehaviour, ISegment
{
    private QuestEditor QuestEditor { get { return (QuestEditor)DataEditor; } }

    private DataManager dataManager = new DataManager();

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

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

        var searchProperties = new SearchProperties(Enums.DataType.PhaseInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseInteractable>().First();
        searchParameters.phaseId = new List<int>() { QuestEditor.QuestData.PhaseId };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);

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
