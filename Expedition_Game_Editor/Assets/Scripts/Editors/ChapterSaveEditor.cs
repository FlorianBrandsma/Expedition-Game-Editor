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

    public bool Loaded                              { get; set; }

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

    public string EditorNotes
    {
        get { return chapterSaveData.EditorNotes; }
    }

    public string GameNotes
    {
        get { return chapterSaveData.GameNotes; }
    }
    #endregion

    public void InitializeEditor()
    {
        chapterSaveData = (ChapterSaveData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Addable()
    {
        return true;
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return true;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyChapterSaveChanges(dataRequest);
    }

    private void ApplyChapterSaveChanges(DataRequest dataRequest)
    {
        if (EditData.ExecuteType == Enums.ExecuteType.Update)
            UpdateChapterSave(dataRequest);
    }

    private void UpdateChapterSave(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
