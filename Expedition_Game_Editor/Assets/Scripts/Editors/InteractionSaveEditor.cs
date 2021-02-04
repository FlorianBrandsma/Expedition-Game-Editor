using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionSaveEditor : MonoBehaviour, IEditor
{
    private InteractionSaveData interactionSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == interactionSaveData.Id).FirstOrDefault(); } }

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

    public string Name
    {
        get { return Default ? "Default" : TimeManager.FormatTime(StartTime) + " - " + TimeManager.FormatTime(EndTime); }
    }

    #region Data properties
    public int Id
    {
        get { return interactionSaveData.Id; }
    }

    public bool Default
    {
        get { return interactionSaveData.Default; }
    }

    public int StartTime
    {
        get { return interactionSaveData.StartTime; }
    }

    public int EndTime
    {
        get { return interactionSaveData.EndTime; }
    }

    public bool Complete
    {
        get { return interactionSaveData.Complete; }
        set
        {
            interactionSaveData.Complete = value;

            DataList.ForEach(x => ((InteractionSaveElementData)x).Complete = value);
        }
    }

    public string PublicNotes
    {
        get { return interactionSaveData.PublicNotes; }
    }

    public string PrivateNotes
    {
        get { return interactionSaveData.PrivateNotes; }
    }
    #endregion

    public void InitializeEditor()
    {
        interactionSaveData = (InteractionSaveData)ElementData.Clone();
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
        ApplyInteractionSaveChanges(dataRequest);
    }

    private void ApplyInteractionSaveChanges(DataRequest dataRequest)
    {
        if (EditData.ExecuteType == Enums.ExecuteType.Update)
            UpdateInteractionSave(dataRequest);
    }

    private void UpdateInteractionSave(DataRequest dataRequest)
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
