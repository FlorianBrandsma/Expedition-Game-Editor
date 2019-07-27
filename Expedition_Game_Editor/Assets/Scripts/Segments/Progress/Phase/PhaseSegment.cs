using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.editorController.PathController.loaded) return;

        var searchParameters = new Search.Phase();

        searchParameters.chapterId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Chapter).GeneralData().id };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void ApplySegment() { }
    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
