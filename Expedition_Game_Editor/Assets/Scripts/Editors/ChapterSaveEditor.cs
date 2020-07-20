using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterSaveEditor : MonoBehaviour, IEditor
{
    public ChapterSaveElementData ChapterSaveData { get { return (ChapterSaveElementData)Data.elementData; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(ChapterSaveData).Concat(new[] { ChapterSaveData }).Distinct().ToList(); }
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
        ChapterSaveData.Update();

        ElementDataList.ForEach(x =>
        {
            if (((GeneralData)x).Equals(ChapterSaveData))
                x.Copy(ChapterSaveData);
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
