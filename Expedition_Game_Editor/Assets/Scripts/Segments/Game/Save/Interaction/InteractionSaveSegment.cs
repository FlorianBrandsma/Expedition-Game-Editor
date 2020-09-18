using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InteractionSaveSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.InteractionSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionSave>().First();
        searchParameters.taskSaveId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.TaskSave).ElementData.Id };

        SegmentController.DataController.GetData(searchProperties);
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

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
