using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class OutcomeSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties { get { return GetComponent<ListProperties>(); } }

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

        var searchProperties = new SearchProperties(Enums.DataType.Outcome);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Outcome>().First();
        searchParameters.interactionId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Interaction).GeneralData.Id };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
