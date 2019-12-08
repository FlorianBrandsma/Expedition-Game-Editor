using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
    #region Methods

    static public void SetIndex(List<GeneralData> dataList)
    {
        for (int index = 0; index < dataList.Count; index++)
        {
            var data = dataList[index];

            data.Index = index;
        }
    }

    #endregion

    #region Functions

    public List<IconData> GetIconData(Search.Icon searchParameters)
    {
        List<IconData> dataList = new List<IconData>();

        foreach (Fixtures.Icon icon in Fixtures.iconList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(icon.Id)) continue;

            var data = new IconData();

            data.Id = icon.Id;

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objectGraphic.Id)) continue;

            var data = new ObjectGraphicData();

            data.Id = objectGraphic.Id;

            data.iconId = objectGraphic.iconId;

            data.name = objectGraphic.name;
            data.path = objectGraphic.path;
            
            data.height = objectGraphic.height;
            data.width = objectGraphic.width;
            data.depth = objectGraphic.depth;

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interactable.Id)) continue;

            var data = new InteractableData();
            
            data.Id = interactable.Id;

            data.objectGraphicId = interactable.objectGraphicId;
            data.name = interactable.name;

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

            data.Id = sceneInteractable.Id;

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

        foreach (Fixtures.Phase phase in Fixtures.phaseList)
        {
            if (searchById && chapterId != phase.chapterId) continue;

            var data = new PhaseData();

            data.Id = phase.Id;
            data.chapterId = phase.chapterId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<PhaseInteractableData> GetPhaseInteractableData(Search.PhaseInteractable searchParameters)
    {
        List<PhaseInteractableData> dataList = new List<PhaseInteractableData>();

        foreach (Fixtures.PhaseInteractable phaseInteractable in Fixtures.phaseInteractableList)
        {
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(phaseInteractable.phaseId)) continue;
            if (searchParameters.sceneInteractableId.Count > 0 && !searchParameters.sceneInteractableId.Contains(phaseInteractable.sceneInteractableId)) continue;

            var data = new PhaseInteractableData();

            data.Id = phaseInteractable.Id;

            data.phaseId = phaseInteractable.phaseId;
            data.questId = phaseInteractable.questId;
            data.sceneInteractableId = phaseInteractable.sceneInteractableId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<QuestData> GetQuestData(Search.Quest searchParameters)
    {
        var dataList = new List<QuestData>();

        foreach (Fixtures.Quest quest in Fixtures.questList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(quest.Id)) continue;

            var data = new QuestData();

            data.Id = quest.Id;

            data.phaseId = quest.phaseId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<ObjectiveData> GetObjectiveData(Search.Objective searchParameters)
    {
        var dataList = new List<ObjectiveData>();

        foreach (Fixtures.Objective objective in Fixtures.objectiveList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objective.Id)) continue;

            var data = new ObjectiveData();

            data.Id = objective.Id;

            data.questId = objective.questId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<SceneInteractableData> GetSceneInteractableData(Search.SceneInteractable searchParameters)
    {
        List<SceneInteractableData> dataList = new List<SceneInteractableData>();

        foreach (Fixtures.SceneInteractable sceneInteractable in Fixtures.sceneInteractableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(sceneInteractable.Id)) continue;

            var data = new SceneInteractableData();

            data.Id = sceneInteractable.Id;

            data.chapterId = sceneInteractable.chapterId;
            data.objectiveId = sceneInteractable.objectiveId;
            data.interactableId = sceneInteractable.interactableId;
            data.interactionIndex = sceneInteractable.interactionIndex;

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
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(interaction.objectiveId)) continue;

            var data = new InteractionData();

            data.Id = interaction.Id;
            data.Index = interaction.Index;

            data.objectiveId = interaction.objectiveId;
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
    
    public List<TileSetData> GetTileSetData()
    {
        return GetTileSetData(new Search.TileSet());
    }

    public List<TileSetData> GetTileSetData(Search.TileSet searchParameters)
    {
        var dataList = new List<TileSetData>();

        foreach(Fixtures.TileSet tileSet in Fixtures.tileSetList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tileSet.Id)) continue;

            var data = new TileSetData();

            data.Id = tileSet.Id;
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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tile.Id)) continue;

            var data = new TileData();

            data.Id = tile.Id;
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

            data.Id = terrainTile.Id;
            data.Index = terrainTile.Index;

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

            data.Id = terrain.Id;
            data.Index = terrain.Index;

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(region.Id)) continue;

            var data = new RegionData();
            
            data.Id = region.Id;

            data.tileSetId = region.tileSetId;

            data.regionSize = region.regionSize;
            data.terrainSize = region.terrainSize;

            data.name = region.name;

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

            data.Id = sceneObject.Id;
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
        public float height;
        public float width;
        public float depth;
    }

    public class InteractableData : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }
    
    public class PhaseData : GeneralData
    {
        public int chapterId;
    }

    public class PhaseInteractableData : GeneralData
    {
        public int phaseId;
        public int questId;
        public int sceneInteractableId;
    }

    public class QuestData : GeneralData
    {
        public int phaseId;
    }

    public class ObjectiveData : GeneralData
    {
        public int questId;
    }

    public class SceneInteractableData : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int interactableId;
        public int interactionIndex;
    }

    public class InteractionData : GeneralData
    {
        public int sceneInteractableId;
        public int objectiveId;
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