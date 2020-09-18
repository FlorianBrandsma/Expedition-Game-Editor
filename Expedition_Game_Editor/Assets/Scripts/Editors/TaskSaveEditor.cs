﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskSaveEditor : MonoBehaviour, IEditor
{
    private TaskSaveData taskSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == taskSaveData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    #region Data properties
    public int Id
    {
        get { return taskSaveData.Id; }
    }
    
    public bool Complete
    {
        get { return taskSaveData.Complete; }
        set
        {
            taskSaveData.Complete = value;

            DataList.ForEach(x => ((TaskSaveElementData)x).Complete = value);
        }
    }

    public string Name
    {
        get { return taskSaveData.Name; }
    }

    public string PublicNotes
    {
        get { return taskSaveData.PublicNotes; }
    }

    public string PrivateNotes
    {
        get { return taskSaveData.PrivateNotes; }
    }
    #endregion

    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });

            return list;
        }
    }

    public void InitializeEditor()
    {
        taskSaveData = (TaskSaveData)ElementData.Clone();
    }

    public void OpenEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        EditData.Update();

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
