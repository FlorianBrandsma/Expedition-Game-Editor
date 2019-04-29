using UnityEngine;
using System.Collections;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }

    private PhaseDataElement phaseData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        phaseData = data.element.Cast<PhaseDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            phaseData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    //public void UpdateElement()
    //{
    //    selectionElement.UpdateElement();
    //}

    public void UpdateIndex(int index)
    {
        var list = data.controller.dataList.Cast<PhaseDataElement>().ToList();

        list.RemoveAt(phaseData.index);
        list.Insert(index, phaseData);

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

    }

    public bool Changed()
    {
        return phaseData.changed;
    }

    public void ApplyChanges()
    {
        phaseData.Update();

        UpdateList();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        EditorManager.editorManager.PreviousEditor();
    }

    public void CloseEditor()
    {

    }
}
