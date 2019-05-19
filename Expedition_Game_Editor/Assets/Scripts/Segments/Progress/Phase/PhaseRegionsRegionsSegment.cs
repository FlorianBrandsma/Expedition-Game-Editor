using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseRegionsRegionsSegment : MonoBehaviour, ISegment
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

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        InitializeRegionData();
    }

    private void InitializeRegionData()
    {
        var phaseEditor = (PhaseEditor)DataEditor;

        if (phaseEditor.phaseRegionDataList.Count > 0) return;

        PhaseDataElement phaseData = DataEditor.Data.ElementData.Cast<PhaseDataElement>().FirstOrDefault();

        var searchParameters = new Search.PhaseRegion();

        searchParameters.requestType = Search.PhaseRegion.RequestType.Custom;
        searchParameters.phaseId = new List<int>() { phaseData.id };
        searchParameters.temp_id_count = 15;

        SegmentController.DataController.GetData(new[] { searchParameters });

        var chapterRegionList = SegmentController.DataController.DataList.Cast<PhaseRegionDataElement>().ToList();
        chapterRegionList.ForEach(x => phaseEditor.phaseRegionDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
