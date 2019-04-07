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

    private void InitializeEditor()
    {
        if (pathController.dataEditor != null)
            pathController.dataEditor.InitializeEditor();
    }

    private void InitializeSegments()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeSegment(this);
    }

    public void InitializeDisplay()
    {
        InitializeSegmentDisplay();
    }

    public void SetDisplay()
    {
        SetSegmentDisplay();
    }

    public void CloseDisplay()
    {
        SelectionManager.CancelSelection(pathController.route);

        CloseSegmentDisplay();
    }

    private void InitializeSegmentDisplay()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeSegmentDisplay();
    }

    private void SetSegmentDisplay()
    {
        foreach (SegmentController segment in segments)
            segment.SetSegmentDisplay();
    }

    private void CloseSegmentDisplay()
    {
        foreach (SegmentController segment in segments)
            segment.CloseSegmentDisplay();
    }

    public void FinalizeController()
    {
        if (GetComponent<ListProperties>() != null)
        {
            if (GetComponent<ListProperties>().selectionType == SelectionManager.Type.Automatic)
                GetComponent<ListProperties>().AutoSelectElement();
        }

        //path.type = Path.Type.Loaded;
    }
}
