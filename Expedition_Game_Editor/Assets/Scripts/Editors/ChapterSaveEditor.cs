using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterSaveEditor : MonoBehaviour, IEditor
{
    private ChapterSaveData chapterSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == chapterSaveData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return chapterSaveData.Id; }
    }
    
    public bool Complete
    {
        get { return chapterSaveData.Complete; }
        set
        {
            chapterSaveData.Complete = value;

            DataList.ForEach(x => ((ChapterSaveElementData)x).Complete = value);
        }
    }

    public string Name
    {
        get { return chapterSaveData.Name; }
    }

    public string PublicNotes
    {
        get { return chapterSaveData.PublicNotes; }
    }

    public string PrivateNotes
    {
        get { return chapterSaveData.PrivateNotes; }
    }
    #endregion

    public void InitializeEditor()
    {
        chapterSaveData = (ChapterSaveData)ElementData.Clone();
    }

    public void OpenEditor() { }

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

    public void ApplyChanges(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
