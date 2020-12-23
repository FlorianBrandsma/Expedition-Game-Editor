using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveGeneralInteractableSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private ObjectiveEditor ObjectiveEditor     { get { return (ObjectiveEditor)DataEditor; } }

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

        searchParameters.objectiveId = new List<int>() { ObjectiveEditor.Id };
        searchParameters.type = new List<int>() { (int)Enums.InteractableType.Object };

        SegmentController.DataController.GetData(searchProperties);

        var worldInteractableList = SegmentController.DataController.Data.dataList.Cast<WorldInteractableElementData>().ToList();
        worldInteractableList.ForEach(x => ObjectiveEditor.worldInteractableElementDataList.Add(x));
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        SetSearchWorldInteractable((WorldInteractableElementData)mergedElementData);

        DataEditor.UpdateEditor();
    }

    private void SetSearchWorldInteractable(WorldInteractableElementData resultElementData)
    {
        var elementData = ObjectiveEditor.worldInteractableElementDataList.Where(x => x.Id == resultElementData.Id).First();

        elementData.InteractableId      = resultElementData.InteractableId;
        elementData.InteractableName    = resultElementData.InteractableName;
        elementData.ModelIconPath       = resultElementData.ModelIconPath;
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
