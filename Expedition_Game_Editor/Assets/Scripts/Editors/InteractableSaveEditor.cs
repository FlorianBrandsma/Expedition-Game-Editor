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

    public void InitializeEditor()
    {
        interactableSaveData = (InteractableSaveData)ElementData.Clone();
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
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != 0).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
