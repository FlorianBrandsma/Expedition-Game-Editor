using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RegionEditor : MonoBehaviour, IEditor
{
    private RegionData regionData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == regionData.Id).FirstOrDefault(); } }

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
        get { return regionData.Id; }
    }

    public int TileSetId
    {
        get { return regionData.TileSetId; }
        set
        {
            regionData.TileSetId = value;

            DataList.ForEach(x => ((RegionElementData)x).TileSetId = value);
        }
    }

    public int Index
    {
        get { return regionData.Index; }
        set
        {
            regionData.Index = value;

            DataList.ForEach(x => ((RegionElementData)x).Index = value);
        }
    }

    public string Name
    {
        get { return regionData.Name; }
        set
        {
            regionData.Name = value;

            DataList.ForEach(x => ((RegionElementData)x).Name = value);
        }
    }

    public int RegionSize
    {
        get { return regionData.RegionSize; }
        set
        {
            regionData.RegionSize = value;

            DataList.ForEach(x => ((RegionElementData)x).RegionSize = value);
        }

    }
    public int TerrainSize
    {
        get { return regionData.TerrainSize; }
        set
        {
            regionData.TerrainSize = value;

            DataList.ForEach(x => ((RegionElementData)x).TerrainSize = value);
        }
    }

    public string TileIconPath
    {
        get { return regionData.TileIconPath; }
        set
        {
            regionData.TileIconPath = value;

            DataList.ForEach(x => ((RegionElementData)x).TileIconPath = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        regionData = (RegionData)ElementData.Clone();
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
        ApplyRegionChanges(dataRequest);
    }

    private void ApplyRegionChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddRegion(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateRegion(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveRegion(dataRequest);
                break;
        }
    }

    private void AddRegion(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            regionData.Id = tempData.Id;
    }

    private void UpdateRegion(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveRegion(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
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

    //private void ChangedName()
    //{
    //    var chapterRegions = Fixtures.chapterRegionList.Where(x => x.RegionId == regionData.Id).Distinct().ToList();
    //    var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.Id).Contains(x.ChapterRegionId)).Distinct().ToList();

    //    regions.ForEach(x => x.Name = regionData.Name);
    //}

    //private void ChangedTileSet()
    //{
    //    var chapterRegions = Fixtures.chapterRegionList.Where(x => x.RegionId == regionData.Id).Distinct().ToList();
    //    var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.Id).Contains(x.ChapterRegionId)).Distinct().ToList();

    //    regions.ForEach(x => x.TileSetId = regionData.TileSetId);

    //    regions.Add(Fixtures.regionList.Where(x => x.Id == regionData.Id).FirstOrDefault());

    //    var terrains = Fixtures.terrainList.Where(x => regions.Select(y => y.Id).Distinct().ToList().Contains(x.RegionId)).Distinct().ToList();
    //    var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.Id).Distinct().ToList().Contains(x.TerrainId)).Distinct().ToList();
    //    var firstTile = Fixtures.tileList.Where(x => x.TileSetId == regionData.TileSetId).FirstOrDefault();

    //    terrainTiles.ForEach(x => x.TileId = firstTile.Id);

    //    //var interactions = Fixtures.interactionList.Where(x => regions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();

    //    //Fixtures.worldInteractableList.RemoveAll(x => interactions.Where(y => y.objectiveId == 0).Select(y => y.worldInteractableId).Contains(x.Id));
    //    //Fixtures.worldObjectList.RemoveAll(x => regions.Select(y => y.Id).Contains(x.regionId));

    //    //Fixtures.interactionList.RemoveAll(x => interactions.Where(y => y.objectiveId == 0).Select(y => y.Id).Contains(x.Id));
    //}

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
