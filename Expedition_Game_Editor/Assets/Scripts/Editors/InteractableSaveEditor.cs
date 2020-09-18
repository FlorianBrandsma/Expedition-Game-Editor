using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractableSaveEditor : MonoBehaviour, IEditor
{
    private InteractableSaveData interactableSaveData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == interactableSaveData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    #region Data properties
    public int Id
    {
        get { return interactableSaveData.Id; }
    }

    public string InteractableName
    {
        get { return interactableSaveData.InteractableName; }
    }

    public string ModelIconPath
    {
        get { return interactableSaveData.ModelIconPath; }
    }

    public int Health
    {
        get { return interactableSaveData.Health; }
    }

    public int Hunger
    {
        get { return interactableSaveData.Hunger; }
    }

    public int Thirst
    {
        get { return interactableSaveData.Thirst; }
    }

    public float Weight
    {
        get { return interactableSaveData.Weight; }
    }

    public float Speed
    {
        get { return interactableSaveData.Speed; }
    }

    public float Stamina
    {
        get { return interactableSaveData.Stamina; }
    }
    #endregion

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

    public void InitializeEditor()
    {
        interactableSaveData = (InteractableSaveData)ElementData.Clone();
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
