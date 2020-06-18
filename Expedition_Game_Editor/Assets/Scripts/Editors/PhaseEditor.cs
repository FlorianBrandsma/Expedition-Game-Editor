using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    public PhaseDataElement PhaseData { get { return (PhaseDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    public List<RegionDataElement> regionDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(PhaseData).Concat(new[] { PhaseData }).Distinct().ToList(); }
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
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        PhaseData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(PhaseData))
                x.Copy(PhaseData);
            else
                x.Update();

            if (SelectionElementManager.SelectionActive(x.SelectionElement))
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
