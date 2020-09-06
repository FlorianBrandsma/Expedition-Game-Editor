using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RegionEditor : MonoBehaviour, IEditor
{
    public RegionData regionData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataList.Where(x => x.Id == regionData.Id).FirstOrDefault(); } }

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
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        //if (regionData.ChangedName)
        //    ChangedName();

        //if (regionData.ChangedTileSetId)
        //    ChangedTileSet();
        
        EditData.Update();

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    private void ChangedName()
    {
        var chapterRegions = Fixtures.chapterRegionList.Where(x => x.RegionId == regionData.Id).Distinct().ToList();
        var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.Id).Contains(x.ChapterRegionId)).Distinct().ToList();

        regions.ForEach(x => x.Name = regionData.Name);
    }

    private void ChangedTileSet()
    {
        var chapterRegions = Fixtures.chapterRegionList.Where(x => x.RegionId == regionData.Id).Distinct().ToList();
        var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.Id).Contains(x.ChapterRegionId)).Distinct().ToList();

        regions.ForEach(x => x.TileSetId = regionData.TileSetId);

        regions.Add(Fixtures.regionList.Where(x => x.Id == regionData.Id).FirstOrDefault());

        var terrains = Fixtures.terrainList.Where(x => regions.Select(y => y.Id).Distinct().ToList().Contains(x.RegionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.Id).Distinct().ToList().Contains(x.TerrainId)).Distinct().ToList();
        var firstTile = Fixtures.tileList.Where(x => x.TileSetId == regionData.TileSetId).FirstOrDefault();
        
        terrainTiles.ForEach(x => x.TileId = firstTile.Id);
        
        //var interactions = Fixtures.interactionList.Where(x => regions.Select(y => y.Id).Contains(x.regionId)).Distinct().ToList();

        //Fixtures.worldInteractableList.RemoveAll(x => interactions.Where(y => y.objectiveId == 0).Select(y => y.worldInteractableId).Contains(x.Id));
        //Fixtures.worldObjectList.RemoveAll(x => regions.Select(y => y.Id).Contains(x.regionId));

        //Fixtures.interactionList.RemoveAll(x => interactions.Where(y => y.objectiveId == 0).Select(y => y.Id).Contains(x.Id));
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
