using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveEditor : MonoBehaviour, IEditor
{
    private ObjectiveData objectiveData;

    public List<WorldInteractableElementData> worldInteractableElementDataList = new List<WorldInteractableElementData>();

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == objectiveData.Id).FirstOrDefault(); } }

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

            worldInteractableElementDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return objectiveData.Id; }
    }

    public int Index
    {
        get { return objectiveData.Index; }
    }

    public string Name
    {
        get { return objectiveData.Name; }
        set
        {
            objectiveData.Name = value;

            DataList.ForEach(x => ((ObjectiveElementData)x).Name = value);
        }
    }

    public string PublicNotes
    {
        get { return objectiveData.PublicNotes; }
        set
        {
            objectiveData.PublicNotes = value;

            DataList.ForEach(x => ((ObjectiveElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return objectiveData.PrivateNotes; }
        set
        {
            objectiveData.PrivateNotes = value;

            DataList.ForEach(x => ((ObjectiveElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        objectiveData = (ObjectiveData)ElementData.Clone();
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
        worldInteractableElementDataList.Clear();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
