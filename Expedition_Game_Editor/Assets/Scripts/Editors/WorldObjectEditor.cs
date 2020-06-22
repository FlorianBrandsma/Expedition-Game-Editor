using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectEditor : MonoBehaviour, IEditor
{
    public WorldObjectDataElement WorldObjectData { get { return (WorldObjectDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(WorldObjectData).Concat(new[] { WorldObjectData }).Distinct().ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

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
        DataElements.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());

        SetEditor();
    }

    public void UpdateIndex(int index) { }

    public void OpenEditor() { }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        WorldObjectData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(WorldObjectData))
                x.Copy(WorldObjectData);
            else
                x.Update();

            if (SelectionElementManager.SelectionActive(x.DataElement))
                x.DataElement.UpdateElement();
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
