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

        //Probably made as a fix for the cloned element editting issue
        //ResetEditorData();

        PathController.DataEditor.EditorSegments.ForEach(x => 
        {
            x.Segment.InitializeData();
        });
    }

    public void ResetEditorData()
    {
        if (RenderManager.loadType == Enums.LoadType.Normal) return;
        
        var data = PathController.DataEditor.Data;

        //Data controller data list somehow didn't match the data's data list? phase default is still broken
        var elementData = data.dataList.Where(x => x.Id == data.elementData.Id).FirstOrDefault();
        //var elementData = data.dataController.DataList.Where(x => x.Id == data.elementData.Id).FirstOrDefault();

        if (elementData != null)
            data.elementData = elementData;
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
