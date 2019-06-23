using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TaskElementTransformEditSegment : MonoBehaviour, ISegment
{
    private TaskDataElement taskData;

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    private IDataController DataController { get { return GetComponent<IDataController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI

    public SelectionElement editButton;

    #endregion

    #region Data Methods

    private void InitializeEditButton()
    {
        taskData = (TaskDataElement)DataEditor.Data.DataElement;

        var regionData = new RegionDataElement();

        regionData.id = taskData.regionId;
        regionData.dataType = Enums.DataType.Region;
        regionData.type = (int)Enums.RegionType.Task;

        editButton.route.path = SegmentController.editorController.pathController.route.path;

        editButton.InitializeElement(null);

        var searchParameters = new Search.Region();

        searchParameters.phaseId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Phase).GeneralData().id };

        DataController.GetData(new[] { searchParameters });

        editButton.route.data = new Data(DataController, regionData);

        editButton.GetComponentInChildren<Text>().text = taskData.regionName != "" ? "Open " + taskData.regionName : "Set Region";
    }

    #endregion

    #region Segment

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
        InitializeEditButton();
    }

    public void InitializeData()
    {
        
    }

    public void OpenSegment()
    {
        
        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {
        
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }

    #endregion
}
