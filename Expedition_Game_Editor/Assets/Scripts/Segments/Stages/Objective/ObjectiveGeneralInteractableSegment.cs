using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveGeneralInteractableSegment : MonoBehaviour, ISegment
{
    private ObjectiveEditor ObjectiveEditor { get { return (ObjectiveEditor)DataEditor; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        searchParameters.objectiveId = new List<int>() { ObjectiveEditor.objectiveData.Id };
        searchParameters.type = new List<int>() { (int)Enums.InteractableType.Object };
        searchParameters.isDefault = Convert.ToInt32(false);

        SegmentController.DataController.GetData(searchProperties);

        var worldInteractableList = SegmentController.DataController.Data.dataList.Cast<WorldInteractableElementData>().ToList();
        worldInteractableList.ForEach(x => ObjectiveEditor.worldInteractableDataList.Add(x));
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement)
    {
        DataEditor.UpdateEditor();
    }
}
