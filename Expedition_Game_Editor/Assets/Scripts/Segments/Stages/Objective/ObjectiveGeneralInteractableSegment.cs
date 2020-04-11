using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveGeneralInteractableSegment : MonoBehaviour, ISegment
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

        var searchParameters = new Search.WorldInteractable();

        searchParameters.requestType = Search.WorldInteractable.RequestType.Custom;
        searchParameters.objectiveId = new List<int>() { ObjectiveEditor.ObjectiveData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, new[] { searchParameters });

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
