using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AtmosphereEditor : MonoBehaviour, IEditor
{
    private AtmosphereData atmosphereData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == atmosphereData.Id).FirstOrDefault(); } }

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
        get { return atmosphereData.Id; }
    }

    public bool Default
    {
        get { return atmosphereData.Default; }
    }

    public int StartTime
    {
        get { return atmosphereData.StartTime; }
        set
        {
            atmosphereData.StartTime = value;

            DataList.ForEach(x => ((AtmosphereElementData)x).StartTime = value);
        }
    }

    public int EndTime
    {
        get { return atmosphereData.EndTime; }
        set
        {
            atmosphereData.EndTime = value;

            DataList.ForEach(x => ((AtmosphereElementData)x).EndTime = value);
        }
    }
    
    public string PublicNotes
    {
        get { return atmosphereData.PublicNotes; }
        set
        {
            atmosphereData.PublicNotes = value;

            DataList.ForEach(x => ((AtmosphereElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return atmosphereData.PrivateNotes; }
        set
        {
            atmosphereData.PrivateNotes = value;

            DataList.ForEach(x => ((AtmosphereElementData)x).PrivateNotes = value);
        }
    }
    
    public string TerrainName
    {
        get { return atmosphereData.TerrainName; }
    }

    public string RegionName
    {
        get { return atmosphereData.RegionName; }
    }

    public string IconPath
    {
        get { return atmosphereData.IconPath; }
    }

    public string BaseTilePath
    {
        get { return atmosphereData.BaseTilePath; }
    }

    public bool TimeConflict
    {
        get { return atmosphereData.TimeConflict; }
        set
        {
            atmosphereData.TimeConflict = value;

            DataList.ForEach(x => ((AtmosphereElementData)x).TimeConflict = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        atmosphereData = (AtmosphereData)ElementData.Clone();
    }

    public void OpenEditor()
    {
        StartTime       = atmosphereData.StartTime;
        EndTime         = atmosphereData.EndTime;

        PublicNotes     = atmosphereData.PublicNotes;
        PrivateNotes    = atmosphereData.PrivateNotes;

        TimeConflict    = atmosphereData.TimeConflict;
    }

    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed) && !TimeConflict;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        var changedTime =   ((AtmosphereElementData)EditData).ChangedStartTime || 
                            ((AtmosphereElementData)EditData).ChangedEndTime;

        EditData.Update(dataRequest);

        if (changedTime)
        {
            Data.dataList = Data.dataList.OrderByDescending(x => ((AtmosphereElementData)x).Default).ThenBy(x => ((AtmosphereElementData)x).StartTime).ToList();
            SelectionElementManager.UpdateElements(ElementData);

        } else {

            if (SelectionElementManager.SelectionActive(EditData.DataElement))
                EditData.DataElement.UpdateElement();
        }

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
