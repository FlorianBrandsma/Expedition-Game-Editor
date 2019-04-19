﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SegmentController : MonoBehaviour
{
    private ISegment segment { get { return GetComponent<ISegment>(); } }

    public GameObject dataController_parent;

    public IDataController dataController
    {
        get
        {
            if (dataController_parent != null)
                return dataController_parent.GetComponent<IDataController>();
            else
                return GetComponent<IDataController>();
        }
    }

    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public EditorController editorController { get; set; }
    public GeneralData generalData { get; set; }

    public bool loaded { get; set; }

    public bool disable_toggle;
    public Toggle toggle;

    public string segment_name;
    public Text header;
    public GameObject content;

    public SegmentController[] sibling_segments;

    public Path path { get; set; }

    private void Awake()
    {
        if (header == null) return;

        header.text = segment_name;

        if (disable_toggle)
            DisableToggle();
    }

    private void DisableToggle()
    {
        toggle.interactable = false;
        toggle.isOn = true;
        toggle.targetGraphic.color = Color.gray;
    }

    public void ActivateSegment()
    {
        foreach(SegmentController segment in sibling_segments)
        {
            if (segment.toggle.isOn != toggle.isOn)
                segment.toggle.isOn  = toggle.isOn;
        }

        content.SetActive(toggle.isOn);
    }

    public void CloseSegment()
    {
        if(segment != null)
            segment.CloseSegment();
    }

    public void InitializeSegment(EditorController editorController)
    {
        this.editorController = editorController;

        path = editorController.pathController.route.path;
        generalData = editorController.pathController.route.GeneralData();

        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().InitializeSegment();

        if (!editorController.pathController.loaded)
        {
            if (dataController != null)
                dataController.InitializeController();    
        }     
    }

    public void FilterRows(List<GeneralData> list)
    {
        if (GetComponent<ListProperties>() != null)
        {
            GetComponent<ListProperties>().CloseDisplay();
            GetComponent<ListProperties>().segmentController.dataController.data_list = new List<GeneralData>(list);
        }

        SetSegmentDisplay();
    }

    public void InitializeSegmentDisplay()
    {
        if (display != null)
            display.InitializeProperties();
    }

    public void SetSegmentDisplay()
    {
        //This block might belong in a non-display method
        if (GetComponent<ISegment>() != null)
            GetComponent<ISegment>().OpenSegment();
        
        if (display != null)
            display.SetDisplay();
    }

    public void CloseSegmentDisplay()
    {
        if (display != null)
            display.CloseDisplay();      
    }

    public bool AutoSelectElement()
    {
        if (GetComponent<ListProperties>() != null)
        {
            if (GetComponent<ListProperties>().selectionType == SelectionManager.Type.Automatic)
            {
                GetComponent<ListProperties>().AutoSelectElement();

                return true;
            }            
        }

        return false;
    }
}
