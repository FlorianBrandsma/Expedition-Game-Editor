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

    public List<ObjectGraphicData> GetObjectGraphicData(Search.ObjectGraphic searchParameters)
    {
        List<ObjectGraphicData> dataList = new List<ObjectGraphicData>();

        foreach(Fixtures.ObjectGraphic objectGraphic in Fixtures.objectGraphicList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objectGraphic.id)) continue;

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
        return GetInteractableData(new Search.Interactable());
    }

    public List<InteractableData> GetInteractableData(Search.Interactable searchParameters)
    {
        List<InteractableData> dataList = new List<InteractableData>();

        foreach(Fixtures.Interactable interactable in Fixtures.interactableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interactable.id)) continue;

            var data = new InteractableData();
            
            data.id = interactable.id;

            data.objectGraphicId = interactable.objectGraphicId;
            data.name = interactable.name;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<SceneInteractableData> GetSceneInteractableData(Search.SceneInteractable searchParameters)
    {
        List<SceneInteractableData> dataList = new List<SceneInteractableData>();

        foreach(Fixtures.SceneInteractable sceneInteractable in Fixtures.sceneInteractableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(sceneInteractable.id)) continue;

            var data = new SceneInteractableData();

            data.id = sceneInteractable.id;

            data.chapterId = sceneInteractable.chapterId;
            data.objectiveId = sceneInteractable.objectiveId;
            data.interactableId = sceneInteractable.interactableId;
            data.interactionIndex = sceneInteractable.interactionIndex;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<SceneInteractableData> GetChapterSceneInteractableData(int chapterId, bool searchById = false)
    {
        List<SceneInteractableData> dataList = new List<SceneInteractableData>();

        foreach (Fixtures.SceneInteractable sceneInteractable in Fixtures.sceneInteractableList)
        {
            if (searchById && chapterId != sceneInteractable.chapterId) continue;

            var data = new SceneInteractableData();

            data.id = sceneInteractable.id;
            
            data.chapterId = sceneInteractable.chapterId;
            data.objectiveId = sceneInteractable.objectiveId;
            data.interactableId = sceneInteractable.interactableId;
            data.interactionIndex = sceneInteractable.interactionIndex;

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
            data.sceneInteractableId = phaseInteractable.sceneInteractableId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TileSetData> GetTileSetData()
    {
        return GetTileSetData(new Search.TileSet());
    }

    public List<TileSetData> GetTileSetData(Search.TileSet searchParameters)
    {
        var dataList = new List<TileSetData>();

        foreach(Fixtures.TileSet tileSet in Fixtures.tileSetList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tileSet.id)) continue;

            var data = new TileSetData();

            data.id = tileSet.id;
            data.name = tileSet.name;
            data.tileSize = tileSet.tileSize;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TileData> GetTileData(Search.Tile searchParameters)
    {
        var dataList = new List<TileData>();

        foreach (Fixtures.Tile tile in Fixtures.tileList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tile.id)) continue;

            var data = new TileData();

            data.id = tile.id;
            data.tileSetId = tile.tileSetId;
            data.iconPath = tile.iconPath;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainTileData> GetTerrainTileData(Search.TerrainTile searchParameters)
    {
        var dataList = new List<TerrainTileData>();

        foreach (Fixtures.TerrainTile terrainTile in Fixtures.terrainTileList)
        {
            if (searchParameters.terrainId.Count > 0 && !searchParameters.terrainId.Contains(terrainTile.terrainId)) continue;

            var data = new TerrainTileData();

            data.id = terrainTile.id;
            data.index = terrainTile.index;

            data.terrainId = terrainTile.terrainId;
            data.tileId = terrainTile.tileId;
            
            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainData> GetTerrainData(Search.Terrain searchParameters)
    {
        var dataList = new List<TerrainData>();

        foreach (Fixtures.Terrain terrain in Fixtures.terrainList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.regionId)) continue;

            var data = new TerrainData();

            data.id = terrain.id;
            data.index = terrain.index;

            data.regionId = terrain.regionId;
            data.name = terrain.name;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<RegionData> GetRegionData()
    {
        return GetRegionData(new Search.Region());
    }

    public List<RegionData> GetRegionData(Search.Region searchParameters)
    {
        List<RegionData> dataList = new List<RegionData>();

        foreach (Fixtures.Region region in Fixtures.regionList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(region.id)) continue;

            var data = new RegionData();
            
            data.id = region.id;

            data.tileSetId = region.tileSetId;

            data.regionSize = region.regionSize;
            data.terrainSize = region.terrainSize;

            data.name = region.name;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<InteractionData> GetInteractionData(Search.Interaction searchParameters)
    {
        var dataList = new List<InteractionData>();

        foreach (Fixtures.Interaction interaction in Fixtures.interactionList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(interaction.regionId)) continue;

            var data = new InteractionData();

            data.id = interaction.id;
            data.sceneInteractableId = interaction.sceneInteractableId;
            data.regionId = interaction.regionId;
            data.terrainId = interaction.terrainId;
            data.terrainTileId = interaction.terrainTileId;

            data.description = interaction.description;

            data.positionX = interaction.positionX;
            data.positionY = interaction.positionY;
            data.positionZ = interaction.positionZ;

            data.rotationX = interaction.rotationX;
            data.rotationY = interaction.rotationY;
            data.rotationZ = interaction.rotationZ;

            data.scaleMultiplier = interaction.scaleMultiplier;

            data.animation = interaction.animation;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<SceneObjectData> GetSceneObjectData(Search.SceneObject searchParameters)
    {
        var dataList = new List<SceneObjectData>();

        foreach (Fixtures.SceneObject sceneObject in Fixtures.sceneObjectList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(sceneObject.regionId)) continue;

            var data = new SceneObjectData();

            data.id = sceneObject.id;
            data.objectGraphicId = sceneObject.objectGraphicId;
            data.regionId = sceneObject.regionId;
            data.terrainId = sceneObject.terrainId;
            data.terrainTileId = sceneObject.terrainTileId;
            
            data.positionX = sceneObject.positionX;
            data.positionY = sceneObject.positionY;
            data.positionZ = sceneObject.positionZ;

            data.rotationX = sceneObject.rotationX;
            data.rotationY = sceneObject.rotationY;
            data.rotationZ = sceneObject.rotationZ;

            data.scaleMultiplier = sceneObject.scaleMultiplier;

            data.animation = sceneObject.animation;

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

    public class SceneInteractableData : GeneralData
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
        public int sceneInteractableId;
    }

    public class TileSetData : GeneralData
    {
        public string name;
        public float tileSize;
    }

    public class TileData : GeneralData
    {
        public int tileSetId;

        public string iconPath;
    }

    public class RegionData : GeneralData
    {
        public int tileSetId;

        public int regionSize;
        public int terrainSize;

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

    public class InteractionData : GeneralData
    {
        public int sceneInteractableId;
        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public string description;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }

    public class SceneObjectData : GeneralData
    {
        public int objectGraphicId;
        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }
    #endregion
}