using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AtmosphereEditor : MonoBehaviour, IEditor
{
    public AtmosphereData atmosphereData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataList.Where(x => x.Id == atmosphereData.Id).FirstOrDefault(); } }

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
    
    public void InitializeEditor() { }

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
        return ElementDataList.Any(x => x.Changed) && !atmosphereData.TimeConflict;
    }

    //public void ApplyChanges()
    //{
    //    var changedTime = AtmosphereData.ChangedStartTime || AtmosphereData.ChangedEndTime;

    //    AtmosphereData.Update();

    //    //If time was changed, reset the entire editor so that the atmosphere segment may reload.
    //    //Elements don't need to be updated as the reset takes care of that.
    //    if (changedTime)
    //    {
    //        RenderManager.ResetPath(true);

    //    } else {

    //        ElementDataList.ForEach(x =>
    //        {
    //            if (DataManager.Equals(x, AtmosphereData))
    //                x.Copy(AtmosphereData);
    //            else
    //                x.Update();

    //            if (SelectionElementManager.SelectionActive(x.DataElement))
    //                x.DataElement.UpdateElement();
    //        });

    //        UpdateEditor();
    //    }
    //}

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
