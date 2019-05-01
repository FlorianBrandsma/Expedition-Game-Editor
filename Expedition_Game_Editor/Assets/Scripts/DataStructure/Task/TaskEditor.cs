using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class TaskEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }

    private TaskDataElement taskData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        taskData = data.element.Cast<TaskDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            taskData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateElement()
    {
        selectionElement.UpdateElement();
    }

    public void UpdateIndex(int index)
    {
        var list = data.controller.DataList.Cast<TaskDataElement>().ToList();

        list.RemoveAt(taskData.index);
        list.Insert(index, taskData);

        selectionElement.ListManager.listProperties.SegmentController.DataController.DataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        selectionElement.ListManager.UpdateData();
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        pathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return taskData.changed;
    }

    public void ApplyChanges()
    {
        taskData.Update();

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
