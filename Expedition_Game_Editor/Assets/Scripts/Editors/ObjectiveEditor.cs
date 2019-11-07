using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveEditor : MonoBehaviour, IEditor
{
    public ObjectiveDataElement ObjectiveData { get { return (ObjectiveDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    public List<SceneInteractableDataElement> sceneInteractableDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(ObjectiveData).Concat(new[] { ObjectiveData }).ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            sceneInteractableDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public List<SegmentController> EditorSegments
    {
        get { return editorSegments; }
    }

    public void UpdateEditor()
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
        ObjectiveData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(ObjectiveData))
                x.SetOriginalValues();
            else
                x.Update();

            if (x.SelectionElement != null)
                x.SelectionElement.UpdateElement();
        });

        UpdateEditor();
    }

    public void CancelEdit()
    {
        DataElements.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
