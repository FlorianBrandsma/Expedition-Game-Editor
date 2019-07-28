using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
    #region Functions

    public List<IconData> GetIconData(List<int> idList, bool searchById = false)
    {
        List<IconData> dataList = new List<IconData>();

        foreach (Fixtures.Icon icon in Fixtures.iconList)
        {
            if (searchById && !idList.Contains(icon.id)) continue;

            var data = new IconData();

            data.id = icon.id;

            data.category = icon.category;
            data.path = icon.path;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<ObjectGraphicData> GetObjectGraphicData(List<int> idList, bool searchById = false)
    {
        List<ObjectGraphicData> dataList = new List<ObjectGraphicData>();

        foreach(Fixtures.ObjectGraphic objectGraphic in Fixtures.objectGraphicList)
        {
            if (searchById && !idList.Contains(objectGraphic.id)) continue;

            var data = new ObjectGraphicData();

            data.id = objectGraphic.id;
            
            data.name = objectGraphic.name;
            data.path = objectGraphic.path;
            data.iconId = objectGraphic.iconId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<InteractableData> GetInteractableData()
    {
        return GetInteractableData(new List<int>());
    }

    public List<InteractableData> GetInteractableData(List<int> idList, bool searchById = false)
    {
        List<InteractableData> dataList = new List<InteractableData>();

        foreach(Fixtures.Interactable interactable in Fixtures.interactableList)
        {
            if (searchById && !idList.Contains(interactable.id)) continue;

            var data = new InteractableData();
            
            data.id = interactable.id;

            data.objectGraphicId = interactable.objectGraphicId;
            data.name = interactable.name;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainInteractableData> GetTerrainInteractableData(List<int> idList, bool searchById = false)
    {
        List<TerrainInteractableData> dataList = new List<TerrainInteractableData>();

        foreach(Fixtures.TerrainInteractable terrainInteractable in Fixtures.terrainInteractableList)
        {
            if (searchById && !idList.Contains(terrainInteractable.id)) continue;

            var data = new TerrainInteractableData();

            data.id = terrainInteractable.id;

            data.chapterId = terrainInteractable.chapterId;
            data.objectiveId = terrainInteractable.objectiveId;
            data.interactableId = terrainInteractable.interactableId;
            data.interactionIndex = terrainInteractable.interactionIndex;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainInteractableData> GetChapterTerrainInteractableData(int chapterId, bool searchById = false)
    {
        List<TerrainInteractableData> dataList = new List<TerrainInteractableData>();

        foreach (Fixtures.TerrainInteractable terrainInteractable in Fixtures.terrainInteractableList)
        {
            if (searchById && chapterId != terrainInteractable.chapterId) continue;

            var data = new TerrainInteractableData();

            data.id = terrainInteractable.id;
            
            data.chapterId = terrainInteractable.chapterId;
            data.objectiveId = terrainInteractable.objectiveId;
            data.interactableId = terrainInteractable.interactableId;
            data.interactionIndex = terrainInteractable.interactionIndex;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<PhaseData> GetPhaseData(int chapterId, bool searchById = false)
    {
        List<PhaseData> dataList = new List<PhaseData>();

        foreach(Fixtures.Phase phase in Fixtures.phaseList)
        {
            if (searchById && chapterId != phase.chapterId) continue;

            var data = new PhaseData();

            data.id = phase.id;
            data.chapterId = phase.chapterId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<PhaseInteractableData> GetPhaseInteractableData(List<int> idList, bool searchById = false)
    {
        List<PhaseInteractableData> dataList = new List<PhaseInteractableData>();

        foreach(Fixtures.PhaseInteractable phaseInteractable in Fixtures.phaseInteractableList)
        {
            if (searchById && !idList.Contains(phaseInteractable.phaseId)) continue;

            var data = new PhaseInteractableData();

            data.id = phaseInteractable.id;

            data.phaseId = phaseInteractable.phaseId;
            data.terrainInteractableId = phaseInteractable.terrainInteractableId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TileSetData> GetTileSetData()
    {
        var dataList = new List<TileSetData>();

        foreach(Fixtures.TileSet tileSet in Fixtures.tileSetList)
        {
            var data = new TileSetData();

            data.id = tileSet.id;
            data.name = tileSet.name;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TileData> GetTileData(List<int> idList, bool searchById = false)
    {
        var dataList = new List<TileData>();

        foreach (Fixtures.Tile tile in Fixtures.tileList)
        {
            if (searchById && !idList.Contains(tile.id)) continue;

            var data = new TileData();

            data.id = tile.id;
            data.tileSetId = tile.tileSetId;
            data.iconPath = tile.iconPath;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainTileData> GetTerrainTileData(List<int> idList, bool searchById = false)
    {
        var dataList = new List<TerrainTileData>();

        foreach (Fixtures.TerrainTile terrainTile in Fixtures.terrainTileList)
        {
            if (searchById && !idList.Contains(terrainTile.id)) continue;

            var data = new TerrainTileData();

            data.id = terrainTile.id;
            data.terrainId = terrainTile.terrainId;
            
            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainData> GetTerrainData(List<int> idList, bool searchById = false)
    {
        var dataList = new List<TerrainData>();

        foreach (Fixtures.Terrain terrain in Fixtures.terrainList)
        {
            if (searchById && !idList.Contains(terrain.id)) continue;

            var data = new TerrainData();

            data.id = terrain.id;
            data.regionId = terrain.regionId;
            data.name = terrain.name;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<RegionData> GetRegionData()
    {
        return GetRegionData(new List<int>());
    }

    public List<RegionData> GetRegionData(List<int> idList, bool searchById = false)
    {
        List<RegionData> dataList = new List<RegionData>();

        foreach (Fixtures.Region region in Fixtures.regionList)
        {
            if (searchById && !idList.Contains(region.id)) continue;

            var data = new RegionData();

            data.id = region.id;
            data.name = region.name;

            dataList.Add(data);
        }

        return dataList;
    }

    #endregion

    #region Classes

    public class IconData : GeneralData
    {
        public int category;
        public string path;
    }

    public class ObjectGraphicData : GeneralData
    {
        public int iconId;
        public string name;
        public string path;
    }

    public class InteractableData : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }

    public class TerrainInteractableData : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int interactableId;
        public int interactionIndex;
    }

    public class PhaseData : GeneralData
    {
        public int chapterId;
    }

    public class PhaseInteractableData : GeneralData
    {
        public int phaseId;
        public int terrainInteractableId;
    }

    public class TileSetData : GeneralData
    {
        public string name;
    }

    public class TileData : GeneralData
    {
        public int tileSetId;
        public string iconPath;
    }

    public class RegionData : GeneralData
    {
        public string name;
    }

    public class TerrainData : GeneralData
    {
        public int regionId;
        public string name;
    }

    public class TerrainTileData : GeneralData
    {
        public int terrainId;
        public int tileId;
    }
    #endregion
}