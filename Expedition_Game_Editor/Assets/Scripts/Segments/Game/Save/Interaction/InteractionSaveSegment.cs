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

        InitializeSearchParameters(searchProperties);
        
        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionSave>().First();

        var saveElementData = SegmentController.Path.FindLastRoute(Enums.DataType.Save).ElementData;
        searchParameters.saveId = new List<int>() { saveElementData.Id };

        var taskSaveElementData = (TaskSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.TaskSave).ElementData;
        searchParameters.taskId = new List<int>() { taskSaveElementData.TaskId };
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
