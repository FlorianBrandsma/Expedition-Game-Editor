using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveSaveEditor : MonoBehaviour, IEditor
{
    private ObjectiveSaveData objectiveSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == objectiveSaveData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    #region Data properties
    public int Id
    {
        get { return objectiveSaveData.Id; }
    }
    
    public bool Complete
    {
        get { return objectiveSaveData.Complete; }
        set
        {
            objectiveSaveData.Complete = value;

            DataList.ForEach(x => ((ObjectiveSaveElementData)x).Complete = value);
        }
    }

    public string Name
    {
        get { return objectiveSaveData.Name; }
    }

    public string PublicNotes
    {
        get { return objectiveSaveData.PublicNotes; }
    }

    public string PrivateNotes
    {
        get { return objectiveSaveData.PrivateNotes; }
    }
    #endregion

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

    public void InitializeEditor()
    {
        objectiveSaveData = (ObjectiveSaveData)ElementData.Clone();
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

    public void ApplyChanges()
    {
        EditData.Update();

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
