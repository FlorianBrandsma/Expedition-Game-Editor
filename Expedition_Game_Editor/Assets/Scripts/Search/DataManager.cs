using UnityEngine;
using System;
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
        var dataList = new List<IconData>();

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
        var dataList = new List<ObjectGraphicData>();

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
        var dataList = new List<InteractableData>();

        foreach(Fixtures.Interactable interactable in Fixtures.interactableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interactable.Id)) continue;

            var data = new InteractableData();
            
            data.Id = interactable.Id;

            data.objectGraphicId = interactable.objectGraphicId;

            data.name = interactable.name;

            data.scaleMultiplier = interactable.scaleMultiplier;

            data.health = interactable.health;
            data.hunger = interactable.hunger;
            data.thirst = interactable.thirst;

            data.weight = interactable.weight;
            data.speed = interactable.speed;
            data.stamina = interactable.stamina;
            
            dataList.Add(data);
        }

        return dataList;
    }

    public List<ChapterData> GetChapterData(Search.Chapter searchParameters)
    {
        var dataList = new List<ChapterData>();

        foreach (Fixtures.Chapter chapter in Fixtures.chapterList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.Id)) continue;

            var data = new ChapterData();

            data.Id = chapter.Id;
            data.Index = chapter.Index;

            data.name = chapter.name;

            data.timeSpeed = chapter.timeSpeed;

            data.publicNotes = chapter.publicNotes;
            data.privateNotes = chapter.privateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<PartyMemberData> GetPartyMemberData(Search.PartyMember searchParameters)
    {
        var dataList = new List<PartyMemberData>();

        foreach(Fixtures.PartyMember partyMember in Fixtures.partyMemberList)
        {
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(partyMember.chapterId)) continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(partyMember.interactableId)) continue;

            var data = new PartyMemberData();

            data.Id = partyMember.Id;

            data.chapterId = partyMember.chapterId;
            data.interactableId = partyMember.interactableId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<ChapterInteractableData> GetChapterInteractableData(Search.ChapterInteractable searchParameters)
    {
        var dataList = new List<ChapterInteractableData>();

        foreach (Fixtures.ChapterInteractable chapterInteractable in Fixtures.chapterInteractableList)
        {
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(chapterInteractable.chapterId))             continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(chapterInteractable.interactableId))   continue;

            var data = new ChapterInteractableData();

            data.Id = chapterInteractable.Id;

            data.chapterId = chapterInteractable.chapterId;
            data.interactableId = chapterInteractable.interactableId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<PhaseData> GetPhaseData(Search.Phase searchParameters)
    {
        var dataList = new List<PhaseData>();

        foreach (Fixtures.Phase phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(phase.Id))                             continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(phase.chapterId))               continue;
            if (searchParameters.defaultRegionId.Count  > 0 && !searchParameters.defaultRegionId.Contains(phase.defaultRegionId))   continue;

            var data = new PhaseData();

            data.Id = phase.Id;
            data.chapterId = phase.chapterId;

            data.name = phase.name;

            data.defaultRegionId = phase.defaultRegionId;

            data.defaultPositionX = phase.defaultPositionX;
            data.defaultPositionY = phase.defaultPositionY;
            data.defaultPositionZ = phase.defaultPositionZ;

            data.defaultRotationX = phase.defaultRotationX;
            data.defaultRotationY = phase.defaultRotationY;
            data.defaultRotationZ = phase.defaultRotationZ;

            data.defaultTime = phase.defaultTime;

            data.publicNotes = phase.publicNotes;
            data.privateNotes = phase.privateNotes;

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

            data.name = quest.name;

            data.publicNotes = quest.publicNotes;
            data.privateNotes = quest.privateNotes;

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

            data.name = objective.name;

            data.publicNotes = objective.publicNotes;
            data.privateNotes = objective.privateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<WorldInteractableData> GetWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        var dataList = new List<WorldInteractableData>();

        foreach (Fixtures.WorldInteractable worldInteractable in Fixtures.worldInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(worldInteractable.Id))                             continue;
            if (searchParameters.type.Count             > 0 && !searchParameters.type.Contains(worldInteractable.type))                         continue;
            if (searchParameters.objectiveId.Count      > 0 && !searchParameters.objectiveId.Contains(worldInteractable.objectiveId))           continue;

            var data = new WorldInteractableData();

            data.Id = worldInteractable.Id;

            data.type = worldInteractable.type;

            data.phaseId = worldInteractable.phaseId;
            data.questId = worldInteractable.questId;
            data.objectiveId = worldInteractable.objectiveId;

            data.chapterInteractableId = worldInteractable.chapterInteractableId;
            data.interactableId = worldInteractable.interactableId;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TaskData> GetTaskData(Search.Task searchParameters)
    {
        var dataList = new List<TaskData>();

        foreach(Fixtures.Task task in Fixtures.taskList)
        {
            if (searchParameters.id.Count                   > 0 && !searchParameters.id.Contains(task.Id))                                      continue;
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(task.worldInteractableId))    continue;
            if (searchParameters.objectiveId.Count          > 0 && !searchParameters.objectiveId.Contains(task.objectiveId))                    continue;

            var data = new TaskData();

            data.Id = task.Id;
            data.Index = task.Index;

            data.worldInteractableId = task.worldInteractableId;
            data.objectiveId = task.objectiveId;

            data.name = task.name;

            data.completeObjective = task.completeObjective;
            data.repeatable = task.repeatable;

            data.publicNotes = task.publicNotes;
            data.privateNotes = task.privateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<InteractionData> GetInteractionData(Search.Interaction searchParameters)
    {
        var dataList = new List<InteractionData>();

        foreach (Fixtures.Interaction interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(interaction.Id))               continue;
            if (searchParameters.taskId.Count   > 0 && !searchParameters.taskId.Contains(interaction.taskId))       continue;
            
            var data = new InteractionData();

            data.Id = interaction.Id;
            data.Index = interaction.Index;

            data.taskId = interaction.taskId;
            
            data.isDefault = interaction.isDefault;

            data.startTime = interaction.startTime;
            data.endTime = interaction.endTime;

            data.triggerAutomatically = interaction.triggerAutomatically;
            data.beNearDestination = interaction.beNearDestination;
            data.faceAgent = interaction.faceAgent;
            data.facePartyLeader = interaction.facePartyLeader;
            data.hideInteractionIndicator = interaction.hideInteractionIndicator;

            data.interactionRange = interaction.interactionRange;

            data.delayMethod = interaction.delayMethod;
            data.delayDuration = interaction.delayDuration;
            data.hideDelayIndicator = interaction.hideDelayIndicator;

            data.cancelDelayOnInput = interaction.cancelDelayOnInput;
            data.cancelDelayOnMovement = interaction.cancelDelayOnMovement;
            data.cancelDelayOnHit = interaction.cancelDelayOnHit;

            data.publicNotes = interaction.publicNotes;
            data.privateNotes = interaction.privateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<InteractionDestinationData> GetInteractionDestinationData(Search.InteractionDestination searchParameters)
    {
        var dataList = new List<InteractionDestinationData>();

        foreach (Fixtures.InteractionDestination interactionDestination in Fixtures.interactionDestinationList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(interactionDestination.Id))                        continue;
            if (searchParameters.regionId.Count         > 0 && !searchParameters.regionId.Contains(interactionDestination.regionId))            continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(interactionDestination.interactionId))  continue;

            var data = new InteractionDestinationData();

            data.Id = interactionDestination.Id;
            data.Index = interactionDestination.Index;

            data.interactionId = interactionDestination.interactionId;

            data.regionId = interactionDestination.regionId;
            data.terrainId = interactionDestination.terrainId;
            data.terrainTileId = interactionDestination.terrainTileId;

            data.positionX = interactionDestination.positionX;
            data.positionY = interactionDestination.positionY;
            data.positionZ = interactionDestination.positionZ;

            data.positionVariance = interactionDestination.positionVariance;

            data.rotationX = interactionDestination.rotationX;
            data.rotationY = interactionDestination.rotationY;
            data.rotationZ = interactionDestination.rotationZ;

            data.freeRotation = interactionDestination.freeRotation;

            data.animation = interactionDestination.animation;
            data.patience = interactionDestination.patience;

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
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(terrainTile.Id))               continue;
            if (searchParameters.terrainId.Count    > 0 && !searchParameters.terrainId.Contains(terrainTile.terrainId)) continue;

            var data = new TerrainTileData();

            data.Id = terrainTile.Id;
            data.Index = terrainTile.Index;

            data.terrainId = terrainTile.terrainId;
            data.tileId = terrainTile.tileId;
            
            dataList.Add(data);
        }
        
        return dataList;
    }

    public List<AtmosphereData> GetAtmosphereData(Search.Atmosphere searchParameters)
    {
        var dataList = new List<AtmosphereData>();

        foreach(Fixtures.Atmosphere atmosphere in Fixtures.atmosphereList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(atmosphere.Id)) continue;
            if (searchParameters.terrainId.Count > 0 && !searchParameters.terrainId.Contains(atmosphere.terrainId)) continue;

            var data = new AtmosphereData();

            data.Id = atmosphere.Id;

            data.terrainId = atmosphere.terrainId;

            data.isDefault = atmosphere.isDefault;

            data.startTime = atmosphere.startTime;
            data.endTime = atmosphere.endTime;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TerrainData> GetTerrainData(Search.Terrain searchParameters)
    {
        var dataList = new List<TerrainData>();

        foreach (Fixtures.Terrain terrain in Fixtures.terrainList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrain.Id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.regionId)) continue;

            var data = new TerrainData();

            data.Id = terrain.Id;
            data.Index = terrain.Index;

            data.regionId = terrain.regionId;
            data.iconId = terrain.iconId;

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
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(region.Id))            continue;
            if (searchParameters.phaseId.Count  > 0 && !searchParameters.phaseId.Contains(region.phaseId))  continue;

            var data = new RegionData();
            
            data.Id = region.Id;

            data.phaseId = region.phaseId;
            data.tileSetId = region.tileSetId;

            data.regionSize = region.regionSize;
            data.terrainSize = region.terrainSize;

            data.name = region.name;

            dataList.Add(data);
        }

        return dataList;
    }
    
    public List<WorldObjectData> GetWorldObjectData(Search.WorldObject searchParameters)
    {
        var dataList = new List<WorldObjectData>();

        foreach (Fixtures.WorldObject worldObject in Fixtures.worldObjectList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(worldObject.regionId)) continue;

            var data = new WorldObjectData();

            data.Id = worldObject.Id;
            data.objectGraphicId = worldObject.objectGraphicId;
            data.regionId = worldObject.regionId;
            data.terrainId = worldObject.terrainId;
            data.terrainTileId = worldObject.terrainTileId;

            data.positionX = worldObject.positionX;
            data.positionY = worldObject.positionY;
            data.positionZ = worldObject.positionZ;

            data.rotationX = worldObject.rotationX;
            data.rotationY = worldObject.rotationY;
            data.rotationZ = worldObject.rotationZ;

            data.scaleMultiplier = worldObject.scaleMultiplier;

            data.animation = worldObject.animation;
            
            dataList.Add(data);
        }

        return dataList;
    }

    public List<PlayerSaveData> GetPlayerSaveData(Search.PlayerSave searchParameters)
    {
        var dataList = new List<PlayerSaveData>();

        foreach(Fixtures.PlayerSave playerSave in Fixtures.playerSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(playerSave.saveId)) continue;

            var data = new PlayerSaveData();

            data.Id = playerSave.Id;

            data.saveId = playerSave.saveId;
            data.regionId = playerSave.regionId;
            data.partyMemberId = playerSave.partyMemberId;
            
            data.positionX = playerSave.positionX;
            data.positionY = playerSave.positionY;
            data.positionZ = playerSave.positionZ;

            data.gameTime = playerSave.gameTime;
            data.playedSeconds = playerSave.playedTime;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<ChapterSaveData> GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        var dataList = new List<ChapterSaveData>();

        foreach(Fixtures.ChapterSave chapterSave in Fixtures.chapterSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(chapterSave.saveId)) continue;

            var data = new ChapterSaveData();

            data.Id = chapterSave.Id;

            data.saveId = chapterSave.saveId;
            data.chapterId = chapterSave.chapterId;

            data.complete = chapterSave.complete;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<PhaseSaveData> GetPhaseSaveData(Search.PhaseSave searchParameters)
    {
        var dataList = new List<PhaseSaveData>();

        foreach (Fixtures.PhaseSave phaseSave in Fixtures.phaseSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(phaseSave.saveId)) continue;

            var data = new PhaseSaveData();

            data.Id = phaseSave.Id;

            data.saveId = phaseSave.saveId;
            data.chapterSaveId = phaseSave.chapterSaveId;
            data.phaseId = phaseSave.phaseId;

            data.complete = phaseSave.complete;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<QuestSaveData> GetQuestSaveData(Search.QuestSave searchParameters)
    {
        var dataList = new List<QuestSaveData>();

        foreach (Fixtures.QuestSave questSave in Fixtures.questSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(questSave.saveId)) continue;

            var data = new QuestSaveData();

            data.Id = questSave.Id;

            data.saveId = questSave.saveId;
            data.phaseSaveId = questSave.phaseSaveId;
            data.questId = questSave.questId;

            data.complete = questSave.complete;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<ObjectiveSaveData> GetObjectiveSaveData(Search.ObjectiveSave searchParameters)
    {
        var dataList = new List<ObjectiveSaveData>();

        foreach (Fixtures.ObjectiveSave objectiveSave in Fixtures.objectiveSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(objectiveSave.saveId)) continue;

            var data = new ObjectiveSaveData();

            data.Id = objectiveSave.Id;

            data.saveId = objectiveSave.saveId;
            data.questSaveId = objectiveSave.questSaveId;
            data.objectiveId = objectiveSave.objectiveId;

            data.complete = objectiveSave.complete;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<TaskSaveData> GetTaskSaveData(Search.TaskSave searchParameters)
    {
        var dataList = new List<TaskSaveData>();

        foreach (Fixtures.TaskSave taskSave in Fixtures.taskSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(taskSave.saveId)) continue;

            var data = new TaskSaveData();

            data.Id = taskSave.Id;

            data.saveId = taskSave.saveId;
            data.worldInteractableId = taskSave.worldInteractableId;
            data.objectiveSaveId = taskSave.objectiveSaveId;
            data.taskId = taskSave.taskId;

            data.complete = taskSave.complete;

            dataList.Add(data);
        }

        return dataList;
    }

    public List<InteractionSaveData> GetInteractionSaveData(Search.InteractionSave searchParameters)
    {
        var dataList = new List<InteractionSaveData>();

        foreach (Fixtures.InteractionSave interactionSave in Fixtures.interactionSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(interactionSave.saveId)) continue;

            var data = new InteractionSaveData();

            data.Id = interactionSave.Id;

            data.saveId = interactionSave.saveId;
            data.taskSaveId = interactionSave.taskSaveId;
            data.interactionId = interactionSave.interactionId;

            data.complete = interactionSave.complete;

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

        public float scaleMultiplier;

        public int health;
        public int hunger;
        public int thirst;

        public float weight;
        public float speed;
        public float stamina;
    }
    
    public class ChapterData : GeneralData
    {
        public string name;

        public float timeSpeed;

        public string publicNotes;
        public string privateNotes;
    }

    public class PartyMemberData : GeneralData
    {
        public int chapterId;
        public int interactableId;
    }

    public class ChapterInteractableData : GeneralData
    {
        public int chapterId;
        public int interactableId;
    }

    public class PhaseData : GeneralData
    {
        public int chapterId;

        public string name;

        public string publicNotes;
        public string privateNotes;

        public int defaultRegionId;

        public float defaultPositionX;
        public float defaultPositionY;
        public float defaultPositionZ;

        public int defaultRotationX;
        public int defaultRotationY;
        public int defaultRotationZ;

        public int defaultTime;
    }

    public class QuestData : GeneralData
    {
        public int phaseId;

        public string name;

        public string publicNotes;
        public string privateNotes;
    }

    public class ObjectiveData : GeneralData
    {
        public int questId;

        public string name;

        public string publicNotes;
        public string privateNotes;
    }

    public class WorldInteractableData : GeneralData
    {
        public int type;
        
        public int phaseId;
        public int questId;
        public int objectiveId;

        public int chapterInteractableId;
        public int interactableId;
    }

    public class TaskData : GeneralData
    {
        public int worldInteractableId;
        public int objectiveId;

        public string name;

        public bool completeObjective;
        public bool repeatable;

        public string publicNotes;
        public string privateNotes;
    }
    
    public class InteractionData : GeneralData
    {
        public int taskId;

        public bool isDefault;

        public int startTime;
        public int endTime;

        public bool triggerAutomatically;
        public bool beNearDestination;
        public bool faceAgent;
        public bool facePartyLeader;
        public bool hideInteractionIndicator;

        public float interactionRange;

        public int delayMethod;
        public int delayDuration;
        public bool hideDelayIndicator;

        public bool cancelDelayOnInput;
        public bool cancelDelayOnMovement;
        public bool cancelDelayOnHit;
        
        public string publicNotes;
        public string privateNotes;
    }

    public class InteractionDestinationData : GeneralData
    {
        public int interactionId;

        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public float positionVariance;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public bool freeRotation;

        public int animation;
        public float patience;
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
        public int phaseId;
        public int tileSetId;

        public int regionSize;
        public int terrainSize;

        public string name;
    }

    public class TerrainData : GeneralData
    {
        public int regionId;
        public int iconId;
        public string name;
    }

    public class AtmosphereData : GeneralData
    {
        public int terrainId;

        public bool isDefault;

        public int startTime;
        public int endTime;
    }

    public class TerrainTileData : GeneralData
    {
        public int terrainId;
        public int tileId;
    }
    
    public class WorldObjectData : GeneralData
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

    public class PlayerSaveData : GeneralData
    {
        public int saveId;
        public int regionId;
        public int partyMemberId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public int gameTime;
        public int playedSeconds;
    }

    public class ChapterSaveData : GeneralData
    {
        public int saveId;
        public int chapterId;

        public bool complete;
    }

    public class PhaseSaveData : GeneralData
    {
        public int saveId;
        public int chapterSaveId;
        public int phaseId;

        public bool complete;
    }

    public class QuestSaveData : GeneralData
    {
        public int saveId;
        public int phaseSaveId;
        public int questId;

        public bool complete;
    }

    public class ObjectiveSaveData : GeneralData
    {
        public int saveId;
        public int questSaveId;
        public int objectiveId;

        public bool complete;
    }

    public class TaskSaveData : GeneralData
    {
        public int saveId;
        public int worldInteractableId;
        public int objectiveSaveId;
        public int taskId;

        public bool complete;
    }

    public class InteractionSaveData : GeneralData
    {
        public int saveId;
        public int taskSaveId;
        public int interactionId;

        public bool complete;
    }
    #endregion
}