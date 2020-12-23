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

    public bool Loaded { get; set; }

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

    public string PublicNotes
    {
        get { return taskData.PublicNotes; }
        set
        {
            taskData.PublicNotes = value;

            DataList.ForEach(x => ((TaskElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return taskData.PrivateNotes; }
        set
        {
            taskData.PrivateNotes = value;

            DataList.ForEach(x => ((TaskElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        taskData = (TaskData)ElementData.Clone();
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

    public void ApplyChanges(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);

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