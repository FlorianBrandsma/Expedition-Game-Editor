using UnityEngine;
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

        if(!PathController.DataEditor.Loaded)
            PathController.DataEditor.InitializeEditor();

        PathController.DataEditor.EditorSegments.ForEach(x => 
        {
            x.Segment.InitializeData();
        });
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

    public void ResetEditor()
    {
        if (PathController.DataEditor != null)
            PathController.DataEditor.ResetEditor();
    }

    public void CloseSegments()
    {
        foreach (SegmentController segment in segments)
            segment.CloseSegment();
    }
}
