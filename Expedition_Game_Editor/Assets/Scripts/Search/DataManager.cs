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

            dataList.Add(icon);
        }

        return dataList;
    }

    static public List<ModelBaseData> GetModelData(Search.Model searchParameters)
    {
        var dataList = new List<ModelBaseData>();

        foreach(ModelBaseData model in Fixtures.modelList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(model.Id)) continue;

            dataList.Add(model);
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

            dataList.Add(interactable);
        }

        return dataList;
    }

    static public List<ChapterBaseData> GetChapterData(Search.Chapter searchParameters)
    {
        var dataList = new List<ChapterBaseData>();

        foreach (ChapterBaseData chapter in Fixtures.chapterList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.Id)) continue;

            dataList.Add(chapter);
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

            dataList.Add(chapterInteractable);
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

            dataList.Add(phase);
        }

        return dataList;
    }

    static public List<QuestBaseData> GetQuestData(Search.Quest searchParameters)
    {
        var dataList = new List<QuestBaseData>();

        foreach (QuestBaseData quest in Fixtures.questList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(quest.Id)) continue;

            dataList.Add(quest);
        }

        return dataList;
    }

    static public List<ObjectiveBaseData> GetObjectiveData(Search.Objective searchParameters)
    {
        var dataList = new List<ObjectiveBaseData>();

        foreach (ObjectiveBaseData objective in Fixtures.objectiveList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(objective.Id)) continue;

            dataList.Add(objective);
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

            dataList.Add(worldInteractable);
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

            dataList.Add(task);
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

            dataList.Add(interaction);
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

            dataList.Add(outcome);
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

            dataList.Add(interactionDestination);
        }

        return dataList;
    }

    static public List<SceneBaseData> GetSceneData(Search.Scene searchParameters)
    {
        var dataList = new List<SceneBaseData>();

        foreach (SceneBaseData scene in Fixtures.sceneList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(scene.Id))             continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(scene.RegionId)) continue;

            dataList.Add(scene);
        }

        return dataList;
    }

    static public List<SceneActorBaseData> GetSceneActorData(Search.SceneActor searchParameters)
    {
        var dataList = new List<SceneActorBaseData>();

        foreach (SceneActorBaseData sceneActor in Fixtures.sceneActorList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(sceneActor.Id))            continue;
            if (searchParameters.sceneId.Count  > 0 && !searchParameters.sceneId.Contains(sceneActor.SceneId))  continue;

            if (searchParameters.changePosition != null && sceneActor.ChangePosition != searchParameters.changePosition)    continue;

            dataList.Add(sceneActor);
        }

        return dataList;
    }

    static public List<ScenePropBaseData> GetScenePropData(Search.SceneProp searchParameters)
    {
        var dataList = new List<ScenePropBaseData>();

        foreach (ScenePropBaseData sceneProp in Fixtures.scenePropList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(sceneProp.Id))             continue;
            if (searchParameters.sceneId.Count  > 0 && !searchParameters.sceneId.Contains(sceneProp.SceneId))   continue;

            dataList.Add(sceneProp);
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

            dataList.Add(tileSet);
        }

        return dataList;
    }

    static public List<TileBaseData> GetTileData(Search.Tile searchParameters)
    {
        var dataList = new List<TileBaseData>();

        foreach (TileBaseData tile in Fixtures.tileList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(tile.Id)) continue;

            dataList.Add(tile);
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

            dataList.Add(terrainTile);
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

            dataList.Add(atmosphere);
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

            dataList.Add(terrain);
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

            dataList.Add(region);
        }

        return dataList;
    }

    static public List<WorldObjectBaseData> GetWorldObjectData(Search.WorldObject searchParameters)
    {
        var dataList = new List<WorldObjectBaseData>();

        foreach (WorldObjectBaseData worldObject in Fixtures.worldObjectList)
        {
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(worldObject.RegionId)) continue;

            dataList.Add(worldObject);
        }

        return dataList;
    }

    static public List<PlayerSaveBaseData> GetPlayerSaveData(Search.PlayerSave searchParameters)
    {
        var dataList = new List<PlayerSaveBaseData>();

        foreach(PlayerSaveBaseData playerSave in Fixtures.playerSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(playerSave.SaveId)) continue;

            dataList.Add(playerSave);
        }

        return dataList;
    }

    static public List<ChapterSaveBaseData> GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        var dataList = new List<ChapterSaveBaseData>();

        foreach(ChapterSaveBaseData chapterSave in Fixtures.chapterSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(chapterSave.SaveId)) continue;

            dataList.Add(chapterSave);
        }

        return dataList;
    }

    static public List<PhaseSaveBaseData> GetPhaseSaveData(Search.PhaseSave searchParameters)
    {
        var dataList = new List<PhaseSaveBaseData>();

        foreach (PhaseSaveBaseData phaseSave in Fixtures.phaseSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(phaseSave.SaveId)) continue;

            dataList.Add(phaseSave);
        }

        return dataList;
    }

    static public List<QuestSaveBaseData> GetQuestSaveData(Search.QuestSave searchParameters)
    {
        var dataList = new List<QuestSaveBaseData>();

        foreach (QuestSaveBaseData questSave in Fixtures.questSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(questSave.SaveId)) continue;

            dataList.Add(questSave);
        }

        return dataList;
    }

    static public List<ObjectiveSaveBaseData> GetObjectiveSaveData(Search.ObjectiveSave searchParameters)
    {
        var dataList = new List<ObjectiveSaveBaseData>();

        foreach (ObjectiveSaveBaseData objectiveSave in Fixtures.objectiveSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(objectiveSave.SaveId)) continue;

            dataList.Add(objectiveSave);
        }

        return dataList;
    }

    static public List<TaskSaveBaseData> GetTaskSaveData(Search.TaskSave searchParameters)
    {
        var dataList = new List<TaskSaveBaseData>();

        foreach (TaskSaveBaseData taskSave in Fixtures.taskSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(taskSave.SaveId)) continue;

            dataList.Add(taskSave);
        }

        return dataList;
    }

    static public List<InteractionSaveBaseData> GetInteractionSaveData(Search.InteractionSave searchParameters)
    {
        var dataList = new List<InteractionSaveBaseData>();

        foreach (InteractionSaveBaseData interactionSave in Fixtures.interactionSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(interactionSave.SaveId)) continue;

            dataList.Add(interactionSave);
        }

        return dataList;
    }
    #endregion
}