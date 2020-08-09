using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionDestinationEditor : MonoBehaviour, IEditor
{
    public InteractionDestinationElementData InteractionDestinationData { get { return (InteractionDestinationElementData)Data.elementData; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(InteractionDestinationData).Concat(new[] { InteractionDestinationData }).Distinct().ToList(); }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { list.Add(x); });

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
        ElementDataList.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());

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
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        InteractionDestinationData.Update();

        ElementDataList.ForEach(x =>
        {
            if (((GeneralData)x).Equals(InteractionDestinationData))
                x.Copy(InteractionDestinationData);
            else
                x.Update();

            if (SelectionElementManager.SelectionActive(x.DataElement))
                x.DataElement.UpdateElement();
        });

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => 
        {
            x.ClearChanges();
        });

        Loaded = false;
    }

    public void CloseEditor() { }
}
