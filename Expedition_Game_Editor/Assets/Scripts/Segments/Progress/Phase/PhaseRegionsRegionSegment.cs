using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseRegionsRegionSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

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
        var phaseEditor = (PhaseEditor)DataEditor;

        if (phaseEditor.regionDataList.Count > 0) return;

        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        var searchParameters = new Search.Region();

        searchParameters.requestType = Search.Region.RequestType.Custom;
        searchParameters.phaseId = new List<int>() { phaseData.id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
