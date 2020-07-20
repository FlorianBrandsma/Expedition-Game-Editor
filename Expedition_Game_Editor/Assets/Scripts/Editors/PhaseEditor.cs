using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    public PhaseElementData PhaseData { get { return (PhaseElementData)Data.elementData; } }
    
    private List<RegionElementData> regionDataList = new List<RegionElementData>();
    public List<RegionElementData> RegionDataList { get { return regionDataList; } }

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(PhaseData).Concat(new[] { PhaseData }).Distinct().ToList(); }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    private List<SegmentController> editorSegments = new List<SegmentController>();
    public List<SegmentController> EditorSegments { get { return editorSegments; } }

    public void InitializeEditor() { }

    public void UpdateEditor()
    {
        ElementDataList.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());

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
        PhaseData.Update();

        ElementDataList.ForEach(x =>
        {
            if (((GeneralData)x).Equals(PhaseData))
                x.Copy(PhaseData);
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
