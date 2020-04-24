﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveInteractObjectSegment : MonoBehaviour, ISegment
{
    private ObjectiveEditor ObjectiveEditor { get { return (ObjectiveEditor)DataEditor; } }

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

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        searchParameters.objectiveId = new List<int>() { ObjectiveEditor.ObjectiveData.Id };
        searchParameters.type = new List<int>() { (int)Enums.InteractableType.Objects };
        searchParameters.isDefault = Convert.ToInt32(false);

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, searchProperties);

        var worldInteractableList = SegmentController.DataController.DataList.Cast<WorldInteractableDataElement>().ToList();
        worldInteractableList.ForEach(x => ObjectiveEditor.worldInteractableDataList.Add(x));
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