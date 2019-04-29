using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class ElementEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }
    private ElementDataElement elementData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        elementData = data.element.Cast<ElementDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            elementData.ClearChanges();
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
        var list = data.controller.dataList.Cast<ElementDataElement>().ToList();

        list.RemoveAt(elementData.index);
        list.Insert(index, elementData);

        selectionElement.listManager.listProperties.segmentController.dataController.dataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        selectionElement.listManager.UpdateData();
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
        return elementData.changed;
    }

    public void ApplyChanges()
    {
        elementData.Update();

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
