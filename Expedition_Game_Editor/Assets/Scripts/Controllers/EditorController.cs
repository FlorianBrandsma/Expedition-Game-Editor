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

        //Re-assigns the loaded element to an editor in case you return to an opened editor
        //Example: change the model of an interactable with the interaction interactable status editor open
        //When data is obtained, old data still exists; it is not replaced where it was used
        //ResetEditorData();

        if(!PathController.DataEditor.Loaded)
            PathController.DataEditor.InitializeEditor();

        PathController.DataEditor.EditorSegments.ForEach(x => 
        {
            x.Segment.InitializeData();
        });
    }

    public void ResetEditorData()
    {
        //if (RenderManager.loadType == Enums.LoadType.Normal) return;
        
        //var data = PathController.DataEditor.Data;

        //var elementData = data.dataList.Where(x => x.Id == data.elementData.Id).FirstOrDefault();

        //if (elementData != null)
        //    data.elementData = elementData;
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

        if (PathController.DataEditor != null)
            PathController.DataEditor.OpenEditor();
    }

    public void CloseSegments()
    {
        foreach (SegmentController segment in segments)
            segment.CloseSegment();
    }
}
