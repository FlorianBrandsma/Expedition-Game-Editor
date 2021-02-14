using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestEditor : MonoBehaviour, IEditor
{
    private QuestData questData;

    public List<WorldInteractableElementData> worldInteractableElementDataList = new List<WorldInteractableElementData>();

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == questData.Id).FirstOrDefault(); } }

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

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return questData.Id; }
    }

    public int PhaseId
    {
        get { return questData.PhaseId; }
        set
        {
            questData.PhaseId = value;

            DataList.ForEach(x => ((QuestElementData)x).PhaseId = value);
        }
    }

    public int Index
    {
        get { return questData.Index; }
    }

    public string Name
    {
        get { return questData.Name; }
        set
        {
            questData.Name = value;

            DataList.ForEach(x => ((QuestElementData)x).Name = value);
        }
    }

    public string EditorNotes
    {
        get { return questData.EditorNotes; }
        set
        {
            questData.EditorNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).EditorNotes = value);
        }
    }

    public string GameNotes
    {
        get { return questData.GameNotes; }
        set
        {
            questData.GameNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        questData = (QuestData)ElementData.Clone();
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

    public bool Applicable()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return true;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        if (EditData.ExecuteType == Enums.ExecuteType.Add || EditData.ExecuteType == Enums.ExecuteType.Update)
        {
            ApplyQuestChanges(dataRequest);

            ApplyWorldInteractableChanges(dataRequest);
        }

        if (EditData.ExecuteType == Enums.ExecuteType.Remove)
            RemoveQuest(dataRequest);
    }

    private void ApplyQuestChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddQuest(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateQuest(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveQuest(dataRequest);
                break;
        }
    }

    private void AddQuest(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            questData.Id = tempData.Id;

            //Apply new quest id to other enabled elements
            worldInteractableElementDataList.Where(x => x.ElementStatus == Enums.ElementStatus.Enabled).ToList().ForEach(x => x.QuestId = questData.Id);
        }

        if (dataRequest.requestType == Enums.RequestType.Execute)
            questData.Id = tempData.Id;
    }

    private void UpdateQuest(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveQuest(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    private void ApplyWorldInteractableChanges(DataRequest dataRequest)
    {
        //World interactables can only be updated by the quest editor
        foreach (WorldInteractableElementData worldInteractableElementData in worldInteractableElementDataList)
        {
            worldInteractableElementData.Update(dataRequest);
        }
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

        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
