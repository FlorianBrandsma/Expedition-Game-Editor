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
        InitializeSegments();
    }

    void InitializeSegments()
    {
        foreach (SegmentController segment in segments)
            segment.InitializeSegment(this);
    }

    public void OpenEditor()
    {
        if (pathController.dataEditor != null)
            pathController.dataEditor.OpenEditor();

        OpenSegments();
    }

    void OpenSegments()
    {
        foreach (SegmentController segment in segments)
            segment.OpenSegment();
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

    public void CloseEditor()
    {
        //Maybe belongs in segments now?
        SelectionManager.CancelSelection(pathController.route);

        CloseSegments();
    }

    public void CloseSegments()
    {        
        foreach (SegmentController segment in segments)
            segment.CloseSegment(); 
    }
}
