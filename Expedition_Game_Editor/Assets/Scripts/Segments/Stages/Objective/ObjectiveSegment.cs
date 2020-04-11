using UnityEngine;
using System.Collections.Generic;

public class ObjectiveSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchParameters = new Search.Objective();

        searchParameters.questId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Quest).GeneralData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
