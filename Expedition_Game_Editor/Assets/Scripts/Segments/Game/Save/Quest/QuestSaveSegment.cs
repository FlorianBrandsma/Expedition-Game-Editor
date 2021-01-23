using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestSaveSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.QuestSave);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.QuestSave>().First();

        var saveElementData = SegmentController.Path.FindLastRoute(Enums.DataType.Save).ElementData;
        searchParameters.saveId = new List<int>() { saveElementData.Id };

        var phaseSaveElementData = (PhaseSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.PhaseSave).ElementData;
        searchParameters.phaseId = new List<int>() { phaseSaveElementData.PhaseId };
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
