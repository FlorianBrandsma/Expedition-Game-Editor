using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveEditor : MonoBehaviour, IEditor
{
    public ObjectiveElementData ObjectiveData { get { return (ObjectiveElementData)Data.elementData; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    public List<WorldInteractableElementData> worldInteractableDataList = new List<WorldInteractableElementData>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(ObjectiveData).Concat(new[] { ObjectiveData }).Distinct().ToList(); }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => list.Add(x));

            worldInteractableDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public List<SegmentController> EditorSegments
    {
        get { return editorSegments; }
    }

    public void InitializeEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        ObjectiveData.Update();

        ElementDataList.ForEach(x =>
        {
            if (((GeneralData)x).Equals(ObjectiveData))
                x.Copy(ObjectiveData);
            else
                x.Update();

            if (SelectionElementManager.SelectionActive(x.DataElement))
                x.DataElement.UpdateElement();
        });

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
