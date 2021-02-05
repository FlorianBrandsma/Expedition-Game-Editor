using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskEditor : MonoBehaviour, IEditor
{
    private TaskData taskData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == taskData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded                              { get; set; }

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

    #region Data properties
    public int Id
    {
        get { return taskData.Id; }
    }

    public int Index
    {
        get { return taskData.Index; }
    }

    public string Name
    {
        get { return taskData.Name; }
        set
        {
            taskData.Name = value;

            DataList.ForEach(x => ((TaskElementData)x).Name = value);
        }
    }

    public bool CompleteObjective
    {
        get { return taskData.CompleteObjective; }
        set
        {
            taskData.CompleteObjective = value;

            DataList.ForEach(x => ((TaskElementData)x).CompleteObjective = value);
        }
    }

    public bool Repeatable
    {
        get { return taskData.Repeatable; }
        set
        {
            taskData.Repeatable = value;

            DataList.ForEach(x => ((TaskElementData)x).Repeatable = value);
        }
    }

    public string EditorNotes
    {
        get { return taskData.EditorNotes; }
        set
        {
            taskData.EditorNotes = value;

            DataList.ForEach(x => ((TaskElementData)x).EditorNotes = value);
        }
    }

    public string GameNotes
    {
        get { return taskData.GameNotes; }
        set
        {
            taskData.GameNotes = value;

            DataList.ForEach(x => ((TaskElementData)x).GameNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        taskData = (TaskData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Addable()
    {
        return true;
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return true;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyTaskChanges(dataRequest);
    }

    private void ApplyTaskChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddTask(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateTask(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveTask(dataRequest);
                break;
        }
    }

    private void AddTask(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            taskData.Id = tempData.Id;
    }

    private void UpdateTask(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveTask(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}