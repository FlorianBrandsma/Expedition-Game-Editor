using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskSaveEditor : MonoBehaviour, IEditor
{
    private TaskSaveData taskSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == taskSaveData.Id).FirstOrDefault(); } }

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
        get { return taskSaveData.Id; }
    }
    
    public bool Complete
    {
        get { return taskSaveData.Complete; }
        set
        {
            taskSaveData.Complete = value;

            DataList.ForEach(x => ((TaskSaveElementData)x).Complete = value);
        }
    }

    public string Name
    {
        get { return taskSaveData.Name; }
    }

    public string PublicNotes
    {
        get { return taskSaveData.PublicNotes; }
    }

    public string PrivateNotes
    {
        get { return taskSaveData.PrivateNotes; }
    }
    #endregion
    
    public void InitializeEditor()
    {
        taskSaveData = (TaskSaveData)ElementData.Clone();
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

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                UpdateEditor();
                break;
        }
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x =>
        {
            x.ExecuteType = Enums.ExecuteType.Update;
            x.ClearChanges();
        });

        Loaded = false;
    }

    public void CloseEditor() { }
}
