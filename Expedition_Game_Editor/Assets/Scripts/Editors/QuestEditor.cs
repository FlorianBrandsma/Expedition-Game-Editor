using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestEditor : MonoBehaviour, IEditor
{
    public QuestDataElement QuestData { get { return (QuestDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    public List<PhaseInteractableDataElement> questInteractableDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(QuestData).Concat(new[] { QuestData }).Distinct().ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            questInteractableDataList.ForEach(x => list.Add(x));

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
        QuestData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(QuestData))
                x.Copy(QuestData);
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
