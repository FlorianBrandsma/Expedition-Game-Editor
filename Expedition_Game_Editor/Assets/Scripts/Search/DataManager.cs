using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class DataManager
{
    #region Functions
    static public void ReplaceRouteData(IDataController dataController)
    {
        //Replaces data in all active forms
        var activeForms = RenderManager.layoutManager.forms.Where(x => x.activeInPath).ToList();
        var activeRoutes = activeForms.SelectMany(x => x.activePath.routeList).Where(x => x.data != null && x.data.dataController == dataController).ToList();

        activeRoutes.ForEach(x => x.data = dataController.Data);
    }

    static public bool Equals(IElementData currentData, IElementData incomingData)
    {
        if (currentData.DataType != incomingData.DataType)
            return false;

        if (currentData.Id != incomingData.Id)
            return false;

        return true;
    }

    static public List<IconBaseData> GetIconData(Search.Icon searchParameters)
    {
        var dataList = new List<IconBaseData>();

        foreach (IconBaseData icon in Fixtures.iconList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(icon.Id)) continue;

            var data = new IconBaseData();

            data.Id = icon.Id;

            data.Category = icon.Category;
            data.Path = icon.Path;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<ModelBaseData> GetModelData(Search.Model searchParameters)
    {
        var dataList = new List<ModelBaseData>();

        foreach(ModelBaseData model in Fixtures.modelList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(model.Id)) continue;

            var data = new ModelBaseData();

            data.Id = model.Id;

            data.IconId = model.IconId;

            data.Name = model.Name;
            data.Path = model.Path;

            data.Height = model.Height;
            data.Width = model.Width;
            data.Depth = model.Depth;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<InteractableBaseData> GetInteractableData()
    {
        return GetInteractableData(new Search.Interactable());
    }

    static public List<InteractableBaseData> GetInteractableData(Search.Interactable searchParameters)
    {
        var dataList = new List<InteractableBaseData>();

        foreach(InteractableBaseData interactable in Fixtures.interactableList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(interactable.Id))          continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(interactable.Id))    continue;

            var data = new InteractableBaseData();
            
            data.Id = interactable.Id;
            data.Index = interactable.Index;

            data.ModelId = interactable.ModelId;

            data.Name = interactable.Name;

            data.Scale = interactable.Scale;

            data.Health = interactable.Health;
            data.Hunger = interactable.Hunger;
            data.Thirst = interactable.Thirst;

            data.Weight = interactable.Weight;
            data.Speed = interactable.Speed;
            data.Stamina = interactable.Stamina;
            
            dataList.Add(data);
        }

        return dataList;
    }

    static public List<ChapterBaseData> GetChapterData(Search.Chapter searchParameters)
    {
        var dataList = new List<ChapterBaseData>();

        foreach (ChapterBaseData chapter in Fixtures.chapterList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.Id)) continue;

            var data = new ChapterBaseData();

            data.Id = chapter.Id;
            data.Index = chapter.Index;

            data.Name = chapter.Name;

            data.TimeSpeed = chapter.TimeSpeed;

            data.PublicNotes = chapter.PublicNotes;
            data.PrivateNotes = chapter.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<ChapterInteractableBaseData> GetChapterInteractableData(Search.ChapterInteractable searchParameters)
    {
        var dataList = new List<ChapterInteractableBaseData>();

        foreach (ChapterInteractableBaseData chapterInteractable in Fixtures.chapterInteractableList)
        {
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(chapterInteractable.ChapterId))             continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(chapterInteractable.InteractableId))   continue;

            var data = new ChapterInteractableBaseData();

            data.Id = chapterInteractable.Id;

            data.ChapterId = chapterInteractable.ChapterId;
            data.InteractableId = chapterInteractable.InteractableId;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<PhaseBaseData> GetPhaseData(Search.Phase searchParameters)
    {
        var dataList = new List<PhaseBaseData>();

        foreach (PhaseBaseData phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(phase.Id))                             continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(phase.ChapterId))               continue;
            if (searchParameters.defaultRegionId.Count  > 0 && !searchParameters.defaultRegionId.Contains(phase.DefaultRegionId))   continue;

            var data = new PhaseBaseData();

            data.Id = phase.Id;
            data.Index = phase.Index;

            data.ChapterId = phase.ChapterId;

            data.Name = phase.Name;

            data.DefaultRegionId = phase.DefaultRegionId;

            data.DefaultPositionX = phase.DefaultPositionX;
            data.DefaultPositionY = phase.DefaultPositionY;
            data.DefaultPositionZ = phase.DefaultPositionZ;

            data.DefaultRotationX = phase.DefaultRotationX;
            data.DefaultRotationY = phase.DefaultRotationY;
            data.DefaultRotationZ = phase.DefaultRotationZ;

            data.DefaultTime = phase.DefaultTime;

            data.PublicNotes = phase.PublicNotes;
            data.PrivateNotes = phase.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<QuestBaseData> GetQuestData(Search.Quest searchParameters)
    {
        var dataList = new List<QuestBaseData>();

        foreach (QuestBaseData quest in Fixtures.questList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(quest.Id)) continue;

            var data = new QuestBaseData();

            data.Id = quest.Id;
            data.Index = quest.Index;

            data.PhaseId = quest.PhaseId;

            data.Name = quest.Name;

            data.PublicNotes = quest.PublicNotes;
            data.PrivateNotes = quest.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<ObjectiveBaseData> GetObjectiveData(Search.Objective searchParameters)
    {
        var dataList = new List<ObjectiveBaseData>();

        foreach (ObjectiveBaseData objective in Fixtures.objectiveList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objective.Id)) continue;

            var data = new ObjectiveBaseData();

            data.Id = objective.Id;
            data.Index = objective.Index;

            data.QuestId = objective.QuestId;

            data.Name = objective.Name;

            data.PublicNotes = objective.PublicNotes;
            data.PrivateNotes = objective.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<WorldInteractableBaseData> GetWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        var dataList = new List<WorldInteractableBaseData>();

        foreach (WorldInteractableBaseData worldInteractable in Fixtures.worldInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(worldInteractable.Id))                     continue;
            if (searchParameters.type.Count             > 0 && !searchParameters.type.Contains(worldInteractable.Type))                 continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(worldInteractable.ChapterId))       continue;
            if (searchParameters.objectiveId.Count      > 0 && !searchParameters.objectiveId.Contains(worldInteractable.ObjectiveId))   continue;

            var data = new WorldInteractableBaseData();

            data.Id = worldInteractable.Id;
            
            data.ChapterId = worldInteractable.ChapterId;
            data.PhaseId = worldInteractable.PhaseId;
            data.QuestId = worldInteractable.QuestId;
            data.ObjectiveId = worldInteractable.ObjectiveId;

            data.ChapterInteractableId = worldInteractable.ChapterInteractableId;
            data.InteractableId = worldInteractable.InteractableId;

            data.Type = worldInteractable.Type;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<TaskBaseData> GetTaskData(Search.Task searchParameters)
    {
        var dataList = new List<TaskBaseData>();

        foreach(TaskBaseData task in Fixtures.taskList)
        {
            if (searchParameters.id.Count                   > 0 && !searchParameters.id.Contains(task.Id))                                      continue;
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(task.WorldInteractableId))    continue;
            if (searchParameters.objectiveId.Count          > 0 && !searchParameters.objectiveId.Contains(task.ObjectiveId))                    continue;

            var data = new TaskBaseData();

            data.Id = task.Id;
            data.Index = task.Index;

            data.WorldInteractableId = task.WorldInteractableId;
            data.ObjectiveId = task.ObjectiveId;

            data.Name = task.Name;

            data.CompleteObjective = task.CompleteObjective;
            data.Repeatable = task.Repeatable;

            data.PublicNotes = task.PublicNotes;
            data.PrivateNotes = task.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<InteractionBaseData> GetInteractionData(Search.Interaction searchParameters)
    {
        var dataList = new List<InteractionBaseData>();

        foreach (InteractionBaseData interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(interaction.Id))               continue;
            if (searchParameters.taskId.Count   > 0 && !searchParameters.taskId.Contains(interaction.TaskId))       continue;
            
            var data = new InteractionBaseData();

            data.Id = interaction.Id;

            data.TaskId = interaction.TaskId;
            
            data.Default = interaction.Default;

            data.StartTime = interaction.StartTime;
            data.EndTime = interaction.EndTime;

            data.ArrivalType = interaction.ArrivalType;

            data.TriggerAutomatically = interaction.TriggerAutomatically;
            data.BeNearDestination = interaction.BeNearDestination;
            data.FaceInteractable = interaction.FaceInteractable;
            data.FaceControllable = interaction.FaceControllable;
            data.HideInteractionIndicator = interaction.HideInteractionIndicator;

            data.InteractionRange = interaction.InteractionRange;

            data.DelayMethod = interaction.DelayMethod;
            data.DelayDuration = interaction.DelayDuration;
            data.HideDelayIndicator = interaction.HideDelayIndicator;

            data.CancelDelayOnInput = interaction.CancelDelayOnInput;
            data.CancelDelayOnMovement = interaction.CancelDelayOnMovement;
            data.CancelDelayOnHit = interaction.CancelDelayOnHit;

            data.PublicNotes = interaction.PublicNotes;
            data.PrivateNotes = interaction.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<OutcomeBaseData> GetOutcomeData(Search.Outcome searchParameters)
    {
        var dataList = new List<OutcomeBaseData>();

        foreach (OutcomeBaseData outcome in Fixtures.outcomeList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(outcome.Id)) continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(outcome.InteractionId)) continue;

            var data = new OutcomeBaseData();

            data.Id = outcome.Id;

            data.InteractionId = outcome.InteractionId;

            data.Type = outcome.Type;

            data.CompleteTask = outcome.CompleteTask;
            data.ResetObjective = outcome.ResetObjective;

            data.PublicNotes = outcome.PublicNotes;
            data.PrivateNotes = outcome.PrivateNotes;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<InteractionDestinationBaseData> GetInteractionDestinationData(Search.InteractionDestination searchParameters)
    {
        var dataList = new List<InteractionDestinationBaseData>();

        foreach (InteractionDestinationBaseData interactionDestination in Fixtures.interactionDestinationList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(interactionDestination.Id))                        continue;
            if (searchParameters.regionId.Count         > 0 && !searchParameters.regionId.Contains(interactionDestination.RegionId))            continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(interactionDestination.InteractionId))  continue;

            var data = new InteractionDestinationBaseData();

            data.Id = interactionDestination.Id;

            data.InteractionId = interactionDestination.InteractionId;

            data.RegionId = interactionDestination.RegionId;
            data.TerrainId = interactionDestination.TerrainId;
            data.TerrainTileId = interactionDestination.TerrainTileId;

            data.PositionX = interactionDestination.PositionX;
            data.PositionY = interactionDestination.PositionY;
            data.PositionZ = interactionDestination.PositionZ;

            data.PositionVariance = interactionDestination.PositionVariance;

            data.RotationX = interactionDestination.RotationX;
            data.RotationY = interactionDestination.RotationY;
            data.RotationZ = interactionDestination.RotationZ;

            data.FreeRotation = interactionDestination.FreeRotation;

            data.Animation = interactionDestination.Animation;
            data.Patience = interactionDestination.Patience;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<TileSetBaseData> GetTileSetData()
    {
        return GetTileSetData(new Search.TileSet());
    }

    static public List<TileSetBaseData> GetTileSetData(Search.TileSet searchParameters)
    {
        var dataList = new List<TileSetBaseData>();

        foreach(TileSetBaseData tileSet in Fixtures.tileSetList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tileSet.Id)) continue;

            var data = new TileSetBaseData();

            data.Id = tileSet.Id;
            data.Name = tileSet.Name;
            data.TileSize = tileSet.TileSize;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<TileBaseData> GetTileData(Search.Tile searchParameters)
    {
        var dataList = new List<TileBaseData>();

        foreach (TileBaseData tile in Fixtures.tileList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tile.Id)) continue;

            var data = new TileBaseData();

            data.Id = tile.Id;
            data.TileSetId = tile.TileSetId;
            data.IconPath = tile.IconPath;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<TerrainTileBaseData> GetTerrainTileData(Search.TerrainTile searchParameters)
    {
        var dataList = new List<TerrainTileBaseData>();

        foreach (TerrainTileBaseData terrainTile in Fixtures.terrainTileList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(terrainTile.Id))               continue;
            if (searchParameters.terrainId.Count    > 0 && !searchParameters.terrainId.Contains(terrainTile.TerrainId)) continue;

            var data = new TerrainTileBaseData();

            data.Id = terrainTile.Id;
            data.Index = terrainTile.Index;

            data.TerrainId = terrainTile.TerrainId;
            data.TileId = terrainTile.TileId;
            
            dataList.Add(data);
        }
        
        return dataList;
    }

    static public List<AtmosphereBaseData> GetAtmosphereData(Search.Atmosphere searchParameters)
    {
        var dataList = new List<AtmosphereBaseData>();

        foreach(AtmosphereBaseData atmosphere in Fixtures.atmosphereList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(atmosphere.Id)) continue;
            if (searchParameters.terrainId.Count > 0 && !searchParameters.terrainId.Contains(atmosphere.TerrainId)) continue;

            var data = new AtmosphereBaseData();

            data.Id = atmosphere.Id;

            data.TerrainId = atmosphere.TerrainId;

            data.Default = atmosphere.Default;

            data.StartTime = atmosphere.StartTime;
            data.EndTime = atmosphere.EndTime;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<TerrainBaseData> GetTerrainData(Search.Terrain searchParameters)
    {
        var dataList = new List<TerrainBaseData>();

        foreach (TerrainBaseData terrain in Fixtures.terrainList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrain.Id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.RegionId)) continue;

            var data = new TerrainBaseData();

            data.Id = terrain.Id;
            data.Index = terrain.Index;

            data.RegionId = terrain.RegionId;
            data.IconId = terrain.IconId;

            data.Name = terrain.Name;
            
            dataList.Add(data);
        }

        return dataList;
    }

    static public List<RegionBaseData> GetRegionData()
    {
        return GetRegionData(new Search.Region());
    }

    static public List<RegionBaseData> GetRegionData(Search.Region searchParameters)
    {
        var dataList = new List<RegionBaseData>();

        foreach (RegionBaseData region in Fixtures.regionList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(region.Id))            continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(region.Id))      continue;
            if (searchParameters.phaseId.Count      > 0 && !searchParameters.phaseId.Contains(region.PhaseId))  continue;

            var data = new RegionBaseData();
            
            data.Id = region.Id;

            data.PhaseId = region.PhaseId;
            data.TileSetId = region.TileSetId;

            data.RegionSize = region.RegionSize;
            data.TerrainSize = region.TerrainSize;

            data.Name = region.Name;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<WorldObjectBaseData> GetWorldObjectData(Search.WorldObject searchParameters)
    {
        var dataList = new List<WorldObjectBaseData>();

        foreach (WorldObjectBaseData worldObject in Fixtures.worldObjectList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(worldObject.RegionId)) continue;

            var data = new WorldObjectBaseData();

            data.Id = worldObject.Id;

            data.ModelId = worldObject.ModelId;
            data.RegionId = worldObject.RegionId;
            data.TerrainId = worldObject.TerrainId;
            data.TerrainTileId = worldObject.TerrainTileId;

            data.PositionX = worldObject.PositionX;
            data.PositionY = worldObject.PositionY;
            data.PositionZ = worldObject.PositionZ;

            data.RotationX = worldObject.RotationX;
            data.RotationY = worldObject.RotationY;
            data.RotationZ = worldObject.RotationZ;

            data.Scale = worldObject.Scale;

            data.Animation = worldObject.Animation;
            
            dataList.Add(data);
        }

        return dataList;
    }

    static public List<PlayerSaveBaseData> GetPlayerSaveData(Search.PlayerSave searchParameters)
    {
        var dataList = new List<PlayerSaveBaseData>();

        foreach(PlayerSaveBaseData playerSave in Fixtures.playerSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(playerSave.SaveId)) continue;

            var data = new PlayerSaveBaseData();

            data.Id = playerSave.Id;

            data.SaveId = playerSave.SaveId;
            data.RegionId = playerSave.RegionId;
            data.WorldInteractableId = playerSave.WorldInteractableId;
            
            data.PositionX = playerSave.PositionX;
            data.PositionY = playerSave.PositionY;
            data.PositionZ = playerSave.PositionZ;

            data.GameTime = playerSave.GameTime;
            //data.PlayedSeconds = playerSave.PlayedTime;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<ChapterSaveBaseData> GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        var dataList = new List<ChapterSaveBaseData>();

        foreach(ChapterSaveBaseData chapterSave in Fixtures.chapterSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(chapterSave.SaveId)) continue;

            var data = new ChapterSaveBaseData();

            data.Id = chapterSave.Id;

            data.SaveId = chapterSave.SaveId;
            data.ChapterId = chapterSave.ChapterId;

            data.Complete = chapterSave.Complete;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<PhaseSaveBaseData> GetPhaseSaveData(Search.PhaseSave searchParameters)
    {
        var dataList = new List<PhaseSaveBaseData>();

        foreach (PhaseSaveBaseData phaseSave in Fixtures.phaseSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(phaseSave.SaveId)) continue;

            var data = new PhaseSaveBaseData();

            data.Id = phaseSave.Id;

            data.SaveId = phaseSave.SaveId;
            data.ChapterSaveId = phaseSave.ChapterSaveId;
            data.PhaseId = phaseSave.PhaseId;

            data.Complete = phaseSave.Complete;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<QuestSaveBaseData> GetQuestSaveData(Search.QuestSave searchParameters)
    {
        var dataList = new List<QuestSaveBaseData>();

        foreach (QuestSaveBaseData questSave in Fixtures.questSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(questSave.SaveId)) continue;

            var data = new QuestSaveBaseData();

            data.Id = questSave.Id;

            data.SaveId = questSave.SaveId;
            data.PhaseSaveId = questSave.PhaseSaveId;
            data.QuestId = questSave.QuestId;

            data.Complete = questSave.Complete;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<ObjectiveSaveBaseData> GetObjectiveSaveData(Search.ObjectiveSave searchParameters)
    {
        var dataList = new List<ObjectiveSaveBaseData>();

        foreach (ObjectiveSaveBaseData objectiveSave in Fixtures.objectiveSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(objectiveSave.SaveId)) continue;

            var data = new ObjectiveSaveBaseData();

            data.Id = objectiveSave.Id;

            data.SaveId = objectiveSave.SaveId;
            data.QuestSaveId = objectiveSave.QuestSaveId;
            data.ObjectiveId = objectiveSave.ObjectiveId;

            data.Complete = objectiveSave.Complete;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<TaskSaveBaseData> GetTaskSaveData(Search.TaskSave searchParameters)
    {
        var dataList = new List<TaskSaveBaseData>();

        foreach (TaskSaveBaseData taskSave in Fixtures.taskSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(taskSave.SaveId)) continue;

            var data = new TaskSaveBaseData();

            data.Id = taskSave.Id;

            data.SaveId = taskSave.SaveId;
            data.WorldInteractableId = taskSave.WorldInteractableId;
            data.ObjectiveSaveId = taskSave.ObjectiveSaveId;
            data.TaskId = taskSave.TaskId;

            data.Complete = taskSave.Complete;

            dataList.Add(data);
        }

        return dataList;
    }

    static public List<InteractionSaveBaseData> GetInteractionSaveData(Search.InteractionSave searchParameters)
    {
        var dataList = new List<InteractionSaveBaseData>();

        foreach (InteractionSaveBaseData interactionSave in Fixtures.interactionSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(interactionSave.SaveId)) continue;

            var data = new InteractionSaveBaseData();

            data.Id = interactionSave.Id;

            data.SaveId = interactionSave.SaveId;
            data.TaskSaveId = interactionSave.TaskSaveId;
            data.InteractionId = interactionSave.InteractionId;

            data.Complete = interactionSave.Complete;

            dataList.Add(data);
        }

        return dataList;
    }
    #endregion
}