﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TaskElementTransformEditSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    private IDataController DataController { get { return GetComponent<IDataController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI

    public SelectionElement editButton;

    #endregion

    #region Data Methods

    private void InitializeData()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    private void InitializeEditButton()
    {
        var regionData = new RegionDataElement();

        regionData.id = 1;
        regionData.table = "Region";
        regionData.type = (int)Enums.RegionType.Task;

        editButton.route.path = SegmentController.editorController.pathController.route.path;

        editButton.InitializeElement();

        var searchParameters = new Search.Region();
        searchParameters.temp_id_count = 15;

        DataController.GetData(new[] { searchParameters });

        editButton.route.data = new Data(DataController, new[] { regionData });

        var taskData = DataEditor.Data.ElementData.Cast<TaskDataElement>().FirstOrDefault();

        editButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(SelectionManager.Property), editButton.selectionProperty) + " " + taskData.regionName;
    }

    #endregion

    #region Segment

    public void InitializeSegment()
    {
        InitializeData();
        InitializeEditButton();
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
