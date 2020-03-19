﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorController : MonoBehaviour
{
    public PathController PathController { get {return GetComponent<PathController>(); } }
    public SegmentController[] 	segments;

    public void InitializeDependencies()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeDependencies(this);
    }

    public void InitializeController()
    {
        InitializeEditor();
        InitializeSegments();

        if (PathController.DataEditor == null) return;

        PathController.DataEditor.Loaded = true;
    }
    
    public void FinalizeController()
    {
        foreach(SegmentController segment in segments)
        {
            if (segment.AutoSelectElement()) break;
        }
    }

    private void InitializeEditor()
    {
        if (PathController.DataEditor == null) return;

        ResetEditorData();

        PathController.DataEditor.EditorSegments.ForEach(x => 
        {
            x.Segment.InitializeData();
        });
    }

    public void ResetEditorData()
    {
        if (EditorManager.loadType == Enums.LoadType.Normal) return;
        
        var data = PathController.DataEditor.Data;

        var dataElement = data.dataController.DataList.Where(x => x.Id == data.dataElement.Id).FirstOrDefault();

        if (dataElement != null)
            data.dataElement = dataElement;
    }

    public void CloseEditor() { }

    private void InitializeSegments()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeSegment(this);
    }

    public void OpenSegments()
    {
        foreach (SegmentController segment in segments)
            segment.OpenSegment();
    }

    public void CloseSegments()
    {
        foreach (SegmentController segment in segments)
            segment.CloseSegment();
    }
}
