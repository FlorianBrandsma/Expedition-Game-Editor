using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    private PhaseDataElement phaseData;
    private SelectionElement selectionElement;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(phaseData);
            
            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        selectionElement = PathController.Origin;

        Data = PathController.route.data;

        phaseData = Data.ElementData.Cast<PhaseDataElement>().FirstOrDefault();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<PhaseDataElement>().ToList();

        list.RemoveAt(phaseData.Index);
        list.Insert(index, phaseData);

        selectionElement.ListManager.listProperties.DataController.DataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
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
        EditorManager.editorManager.PreviousEditor();
    }

    public void CloseEditor()
    {

    }
}
