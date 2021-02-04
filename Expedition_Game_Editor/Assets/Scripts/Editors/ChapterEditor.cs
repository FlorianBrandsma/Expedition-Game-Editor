using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    private ChapterData chapterData;

    public List<WorldInteractableElementData> worldInteractableElementDataList      = new List<WorldInteractableElementData>();
    public List<ChapterInteractableElementData> chapterInteractableElementDataList  = new List<ChapterInteractableElementData>();
    public List<ChapterRegionElementData> chapterRegionElementDataList              = new List<ChapterRegionElementData>();

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == chapterData.Id).FirstOrDefault(); } }

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

            worldInteractableElementDataList.ForEach(x => list.Add(x));
            chapterInteractableElementDataList.ForEach(x => list.Add(x));
            chapterRegionElementDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return chapterData.Id; }
    }

    public int Index
    {
        get { return chapterData.Index; }
    }

    public string Name
    {
        get { return chapterData.Name; }
        set
        {
            chapterData.Name = value;

            DataList.ForEach(x => ((ChapterElementData)x).Name = value);
        }
    }

    public float TimeSpeed
    {
        get { return chapterData.TimeSpeed; }
        set
        {
            chapterData.TimeSpeed = value;

            DataList.ForEach(x => ((ChapterElementData)x).TimeSpeed = value);
        }
    }

    public string PublicNotes
    {
        get { return chapterData.PublicNotes; }
        set
        {
            chapterData.PublicNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return chapterData.PrivateNotes; }
        set
        {
            chapterData.PrivateNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        chapterData = (ChapterData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
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
        if(EditData.ExecuteType == Enums.ExecuteType.Add || EditData.ExecuteType == Enums.ExecuteType.Update)
        {
            ApplyChapterChanges(dataRequest);

            ApplyWorldInteractableChanges(dataRequest);
            ApplyChapterInteractableChanges(dataRequest);
            ApplyRegionChanges(dataRequest);
        }
        
        if(EditData.ExecuteType == Enums.ExecuteType.Remove)
            RemoveChapter(dataRequest);
    }

    private void ApplyChapterChanges(DataRequest dataRequest)
    {
        switch(EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddChapter(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateChapter(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveChapter(dataRequest);
                break;
        }
    }

    private void AddChapter(DataRequest dataRequest)
    {
        //Create temporary data while the other data's id is being changed
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            chapterData.Id = tempData.Id;

            //Apply new chapter id to other elements
            worldInteractableElementDataList.ForEach(x => x.ChapterId = chapterData.Id);
            chapterInteractableElementDataList.ForEach(x => x.ChapterId = chapterData.Id);
            chapterRegionElementDataList.ForEach(x => x.ChapterId = chapterData.Id);
        }
    }

    private void UpdateChapter(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveChapter(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    private void ApplyWorldInteractableChanges(DataRequest dataRequest)
    {
        foreach (WorldInteractableElementData worldInteractableElementData in worldInteractableElementDataList)
        {
            switch(worldInteractableElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    worldInteractableElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    worldInteractableElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    worldInteractableElementData.Remove(dataRequest);
                    break;
            }
        }

        if(dataRequest.requestType == Enums.RequestType.Execute)
            worldInteractableElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove); 
    }

    private void ApplyChapterInteractableChanges(DataRequest dataRequest)
    {
        foreach (ChapterInteractableElementData chapterInteractableElementData in chapterInteractableElementDataList)
        {
            switch (chapterInteractableElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    chapterInteractableElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    chapterInteractableElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    chapterInteractableElementData.Remove(dataRequest);
                    break;
            }
        }

        if (dataRequest.requestType == Enums.RequestType.Execute)
            chapterInteractableElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove);
    }

    private void ApplyRegionChanges(DataRequest dataRequest)
    {
        foreach (ChapterRegionElementData chapterRegionElementData in chapterRegionElementDataList)
        {
            switch (chapterRegionElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    chapterRegionElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    chapterRegionElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    chapterRegionElementData.Remove(dataRequest);
                    break;
            }
        }

        if (dataRequest.requestType == Enums.RequestType.Execute)
            chapterRegionElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove);
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
        worldInteractableElementDataList.Clear();
        chapterInteractableElementDataList.Clear();
        chapterRegionElementDataList.Clear();

        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }    
}
