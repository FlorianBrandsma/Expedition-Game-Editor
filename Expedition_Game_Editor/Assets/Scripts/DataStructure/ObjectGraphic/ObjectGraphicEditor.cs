using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class ObjectGraphicEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }

    private ObjectGraphicDataElement objectGraphicData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        objectGraphicData = data.element.Cast<ObjectGraphicDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            objectGraphicData.ClearChanges();
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
        return objectGraphicData.changed;
    }

    public void ApplyChanges()
    {
        objectGraphicData.Update();

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
