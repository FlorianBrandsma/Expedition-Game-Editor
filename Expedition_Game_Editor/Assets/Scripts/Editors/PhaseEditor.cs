using UnityEngine;
using System.Collections;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    public DataManager.Type data_type { get { return DataManager.Type.Phase; } }
    public IEnumerable data { get; set; }
    public ICollection data_list { get; set; }

    private PhaseDataElement phaseData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public ButtonActionManager buttonActionManager;

    public void InitializeEditor()
    {
        selectionElement = pathController.route.origin.selectionElement;
        SetList();

        data = pathController.route.data;

        phaseData = data.Cast<PhaseDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            phaseData.ClearChanges();

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);
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
        var list = data_list.Cast<PhaseDataElement>().ToList();

        list.RemoveAt(phaseData.index);
        list.Insert(index, phaseData);

        selectionElement.listManager.listProperties.dataController.data_list = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void SetList()
    {
        data_list = selectionElement.listManager.listProperties.dataController.data_list;
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
        if (buttonActionManager != null)
            buttonActionManager.SetButtons(phaseData.changed);
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
        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();
    }
}
