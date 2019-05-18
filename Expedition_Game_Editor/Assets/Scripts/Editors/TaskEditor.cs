using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskEditor : MonoBehaviour, IEditor
{
    private TaskDataElement taskData;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(taskData);

            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        taskData = Data.ElementData.Cast<TaskDataElement>().FirstOrDefault();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<TaskDataElement>().ToList();

        list.RemoveAt(taskData.Index);
        list.Insert(index, taskData);

        Data.DataController.DataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        if (PathController.Origin == null) return;

        PathController.Origin.ListManager.UpdateData();
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        DataElements.ForEach(x => x.Update());

        UpdateList();

        UpdateEditor();
    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {

    }
}
