﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskSaveController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TaskSave; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IElementData> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public TaskSaveController()
    {
        DataManager = new TaskSaveDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(DataElement searchElement, IElementData resultDataElement) { }

    public void ToggleElement(EditorElement editorElement) { }
}
