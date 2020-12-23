using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestSaveEditor : MonoBehaviour, IEditor
{
    private QuestSaveData questSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == questSaveData.Id).FirstOrDefault(); } }

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
        get { return questSaveData.Id; }
    }
    
    public bool Complete
    {
        get { return questSaveData.Complete; }
        set
        {
            questSaveData.Complete = value;

            DataList.ForEach(x => ((QuestSaveElementData)x).Complete = value);
        }
    }

    public string Name
    {
        get { return questSaveData.Name; }
    }

    public string PublicNotes
    {
        get { return questSaveData.PublicNotes; }
    }

    public string PrivateNotes
    {
        get { return questSaveData.PrivateNotes; }
    }
    #endregion

    public void InitializeEditor()
    {
        questSaveData = (QuestSaveData)ElementData.Clone();
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
