using UnityEngine;
using System.Collections;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    public Enums.DataType data_type { get { return Enums.DataType.Phase; } }
    public IEnumerable data { get; set; }
    public ICollection data_list { get; set; }

    private PhaseDataElement phaseData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;
        SetList();

        data = pathController.route.data;

        phaseData = data.Cast<PhaseDataElement>().FirstOrDefault();

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
        var list = data_list.Cast<PhaseDataElement>().ToList();

        list.RemoveAt(phaseData.index);
        list.Insert(index, phaseData);

        selectionElement.listManager.listProperties.segmentController.dataController.data_list = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void SetList()
    {
        data_list = selectionElement.listManager.listProperties.segmentController.dataController.data_list;
    }

    private void UpdateList()
    {
        SetList();
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
