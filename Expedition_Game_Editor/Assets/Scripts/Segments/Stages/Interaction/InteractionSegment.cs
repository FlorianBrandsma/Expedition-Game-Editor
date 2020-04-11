using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InteractionSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();

        CheckTimeSlots();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchParameters = new Search.Interaction();

        searchParameters.taskId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Task).GeneralData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    private void CheckTimeSlots()
    {
        if (SegmentController.Loaded) return;
        
        //Only allow adding new rows if there are time slots available
        ListProperties.enableAdding = TimeManager.TimeFramesAvailable(SegmentController.DataController);
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
