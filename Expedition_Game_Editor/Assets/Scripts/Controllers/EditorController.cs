using UnityEngine;
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

    private void InitializeEditor()
    {
        if (pathController.dataEditor != null)
            pathController.dataEditor.InitializeEditor();
    }

    //Close Editor?

    private void InitializeSegments()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeSegment(this);
    }

    //Close Segments?

    #region Display
    public void InitializeDisplay()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeSegmentDisplay();
    }

    public void SetDisplay()
    {
        foreach (SegmentController segment in segments)
            segment.SetSegmentDisplay();
    }

    public void CloseDisplay()
    {
        foreach (SegmentController segment in segments)
            segment.CloseSegmentDisplay();
    }
    #endregion
}
