using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AtmosphereSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Atmosphere);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Atmosphere>().First();
        searchParameters.terrainId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Terrain).ElementData.Id };

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment()
    {
        InitializeData();

        CheckTimeSlots();
    }

    private void CheckTimeSlots()
    {
        if (SegmentController.Loaded) return;

        //Only allow adding new rows if there are time slots available
        ListProperties.enableAdding = TimeManager.TimeFramesAvailable(SegmentController.DataController);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
