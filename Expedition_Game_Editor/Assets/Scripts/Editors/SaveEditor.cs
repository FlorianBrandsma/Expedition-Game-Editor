using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SaveEditor : MonoBehaviour, IEditor
{
    private SaveData saveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == saveData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public LayoutSection LayoutSection              { get { return PathController.layoutSection; } }
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
        get { return saveData.Id; }
    }

    public int Index
    {
        get { return saveData.Index; }
    }

    public Enums.SaveType SaveType
    {
        get { return saveData.SaveType; }
    }

    public string InteractableName
    {
        get { return saveData.InteractableName; }
    }
    
    public string ModelIconPath
    {
        get { return saveData.ModelIconPath; }
    }

    public string PhaseName
    {
        get { return saveData.PhaseName; }
    }

    public string PhaseGameNotes
    {
        get { return saveData.PhaseGameNotes; }
    }
    #endregion

    public void InitializeEditor()
    {
        saveData = (SaveData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        LayoutSection.SetActionButtons();
    }

    public bool Addable()
    {
        return false;
    }

    public bool Applicable()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return SaveType == Enums.SaveType.Load && saveData.Id > 0;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplySaveChanges(dataRequest);
    }

    private void ApplySaveChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddSave(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateSave(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveSave(dataRequest);
                break;
        }
    }

    private void AddSave(DataRequest dataRequest)
    {
        var tempData = EditData;

        var saveElementData = (SaveElementData)EditData;

        saveElementData.SaveTime = System.DateTime.Now;

        saveElementData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            saveData.Id = tempData.Id;
    }

    private void UpdateSave(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveSave(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Remove:
                OpenDefault();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void OpenDefault()
    {
        RenderManager.loadType = Enums.LoadType.Reload;

        var defaultElement = Data.dataController.Data.dataList.Where(x => x.Id > 0 && x.ExecuteType != Enums.ExecuteType.Remove).FirstOrDefault();

        if (defaultElement != null)
        {
            ((ListManager)EditData.DataElement.DisplayManager).AutoSelectElement(defaultElement.Id);

        } else {

            RenderManager.PreviousPath();
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
