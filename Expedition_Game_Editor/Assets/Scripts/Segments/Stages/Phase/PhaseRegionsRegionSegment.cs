using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseRegionsRegionSegment : MonoBehaviour, ISegment
{
    private PhaseEditor PhaseEditor { get { return (PhaseEditor)DataEditor; } }

    private DataManager dataManager = new DataManager();

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

        var searchProperties = new SearchProperties(Enums.DataType.Region);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();
        searchParameters.phaseId = new List<int>() { PhaseEditor.PhaseData.Id };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);

        var phaseRegionList = SegmentController.DataController.DataList.Cast<RegionDataElement>().ToList();
        phaseRegionList.ForEach(x => PhaseEditor.RegionDataList.Add(x));
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
