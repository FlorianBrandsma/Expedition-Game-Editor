using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectEditor : MonoBehaviour, IEditor
{
    private List<SceneObjectDataElement> sceneObjectDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Route.Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();
            
            sceneObjectDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        var sceneObjectData = (SceneObjectDataElement)Data.dataElement;
        sceneObjectDataList = SelectionElementManager.FindDataElements(sceneObjectData).Cast<SceneObjectDataElement>().ToList();
        
        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        DataElements.ForEach(x => x.SelectionElement.UpdateElement());

        SetEditor();
    }

    public void UpdateIndex(int index) { }

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

        UpdateEditor();
    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {
        //DataElements.ForEach(x => x.SetOriginalValues());
        //DataElements.ForEach(x => x.SelectionElement.SetElement());
    }
}
