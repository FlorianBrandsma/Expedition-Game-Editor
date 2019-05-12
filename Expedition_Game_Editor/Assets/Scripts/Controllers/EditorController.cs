﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorController : MonoBehaviour
{
    public PathController   pathController { get {return GetComponent<PathController>(); } }
    public SegmentController[] 	segments;

    public void InitializeController()
    {
        InitializeEditor();
        InitializeSegments();
    }

    public void FinalizeController()
    {
        foreach(SegmentController segment in segments)
        {
            if (segment.AutoSelectElement()) break;
        }
    }

    public void CloseController()
    {
        CloseEditor();
        CloseSegments();
    }

    private void InitializeEditor()
    {
        if (pathController.dataEditor != null)
            pathController.dataEditor.InitializeEditor();
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

    private void CloseSegments()
    {
        foreach (SegmentController segment in segments)
            segment.CloseSegment();
    }
}
