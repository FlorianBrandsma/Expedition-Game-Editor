using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class ItemEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }
    private ItemDataElement itemData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        itemData = data.element.Cast<ItemDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            itemData.ClearChanges();
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
        var list = data.controller.DataList.Cast<ItemDataElement>().ToList();

        list.RemoveAt(itemData.Index);
        list.Insert(index, itemData);

        selectionElement.ListManager.listProperties.SegmentController.DataController.DataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        if(pathController.route.data.controller == selectionElement.ListManager.listProperties.SegmentController.DataController)
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
        return itemData.changed;
    }

    public void ApplyChanges()
    {
        itemData.Update();

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
