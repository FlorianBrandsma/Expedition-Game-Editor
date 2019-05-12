using UnityEngine;
using System.Collections;
using System.Linq;

public class QuestEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }

    private QuestDataElement questData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        questData = data.ElementData.Cast<QuestDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            questData.ClearChanges();
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
        var list = data.DataController.DataList.Cast<QuestDataElement>().ToList();

        list.RemoveAt(questData.index);
        list.Insert(index, questData);

        selectionElement.ListManager.listProperties.DataController.DataList = list;

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
        return questData.changed;
    }

    public void ApplyChanges()
    {
        questData.Update();

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
