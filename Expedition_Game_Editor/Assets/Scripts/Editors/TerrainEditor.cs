using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainEditor : MonoBehaviour, IEditor
{
    private TerrainData terrainData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == terrainData.Id).FirstOrDefault(); } }

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
        get { return terrainData.Id; }
    }

    public int IconId
    {
        get { return terrainData.IconId; }
        set
        {
            terrainData.IconId = value;

            DataList.ForEach(x => ((TerrainElementData)x).IconId = value);
        }
    }

    public int Index
    {
        get { return terrainData.Index; }
    }
    
    public string Name
    {
        get { return terrainData.Name; }
        set
        {
            terrainData.Name = value;

            DataList.ForEach(x => ((TerrainElementData)x).Name = value);
        }
    }

    public string IconPath
    {
        get { return terrainData.IconPath; }
        set
        {
            terrainData.IconPath = value;

            DataList.ForEach(x => ((TerrainElementData)x).IconPath = value);
        }
    }

    public string BaseTilePath
    {
        get { return terrainData.BaseTilePath; }
        set
        {
            terrainData.BaseTilePath = value;

            DataList.ForEach(x => ((TerrainElementData)x).BaseTilePath = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        terrainData = (TerrainData)ElementData.Clone();
    }

    public void OpenEditor()
    {
        IconId = terrainData.IconId;
        
        Name = terrainData.Name;

        IconPath = terrainData.IconPath;
        BaseTilePath = terrainData.BaseTilePath;
    }

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
        ApplyTerrainChanges(dataRequest);
    }

    private void ApplyTerrainChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Update:
                UpdateTerrain(dataRequest);
                break;
        }
    }

    private void UpdateTerrain(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
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
