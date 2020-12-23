using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestEditor : MonoBehaviour, IEditor
{
    private QuestData questData;

    public List<WorldInteractableElementData> worldInteractableDataList = new List<WorldInteractableElementData>();

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == questData.Id).FirstOrDefault(); } }

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

            worldInteractableDataList.ForEach(x => list.Add(x));

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

    public string PublicNotes
    {
        get { return questData.PublicNotes; }
        set
        {
            questData.PublicNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return questData.PrivateNotes; }
        set
        {
            questData.PrivateNotes = value;

            DataList.ForEach(x => ((ChapterElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        questData = (QuestData)ElementData.Clone();
    }

    public void OpenEditor() { }

    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ElementDataList.ForEach(x => x.Update(dataRequest));

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        worldInteractableDataList.Clear();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
