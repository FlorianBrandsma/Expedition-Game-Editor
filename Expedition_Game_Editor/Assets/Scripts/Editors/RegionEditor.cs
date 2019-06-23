﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RegionEditor : MonoBehaviour, IEditor
{
    private RegionDataElement regionData;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(regionData);

            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        regionData = (RegionDataElement)Data.DataElement;

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<RegionDataElement>().ToList();

        list.RemoveAt(regionData.Index);
        list.Insert(index, regionData);

        Data.DataController.DataList = list.Cast<IDataElement>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        if (PathController.Origin == null) return;

        PathController.Origin.ListManager.UpdateData();
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        if (regionData.changedName)
            ChangedName();

        if (regionData.changedTileSetId)
            ChangedTileSet();
        
        DataElements.ForEach(x => x.Update());

        UpdateList();

        UpdateEditor();
    }

    private void ChangedName()
    {
        var chapterRegions = Fixtures.chapterRegionList.Where(x => x.regionId == regionData.id).Distinct().ToList();
        var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.id).Contains(x.chapterRegionId)).Distinct().ToList();

        regions.ForEach(x => x.name = regionData.Name);
    }

    private void ChangedTileSet()
    {
        var chapterRegions = Fixtures.chapterRegionList.Where(x => x.regionId == regionData.id).Distinct().ToList();
        var regions = Fixtures.regionList.Where(x => chapterRegions.Select(y => y.id).Contains(x.chapterRegionId)).Distinct().ToList();

        regions.ForEach(x => x.tileSetId = regionData.TileSetId);

        regions.Add(Fixtures.regionList.Where(x => x.id == regionData.id).FirstOrDefault());

        var terrains = Fixtures.terrainList.Where(x => regions.Select(y => y.id).Distinct().ToList().Contains(x.regionId)).Distinct().ToList();
        var terrainTiles = Fixtures.terrainTileList.Where(x => terrains.Select(y => y.id).Distinct().ToList().Contains(x.terrainId)).Distinct().ToList();
        var firstTile = Fixtures.tileList.Where(x => x.tileSetId == regionData.TileSetId).FirstOrDefault();
        
        terrainTiles.ForEach(x => x.tileId = firstTile.id);

        var tasks = Fixtures.taskList.Where(x => terrainTiles.Select(y => y.id).Contains(x.terrainTileId)).Distinct().ToList();

        tasks.ForEach(x => x.terrainTileId = 0);
    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {

    }
}