using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneInteractableDataManager
{
    private SceneInteractableController sceneInteractableController;
    private List<SceneInteractableData> sceneInteractableDataList = new List<SceneInteractableData>();

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(SceneInteractableController sceneInteractableController)
    {
        this.sceneInteractableController = sceneInteractableController;
    }

    public List<IDataElement> GetSceneInteractableDataElements(IEnumerable searchParameters)
    {
        var sceneInteractableSearchData = searchParameters.Cast<Search.SceneInteractable>().FirstOrDefault();

        switch (sceneInteractableSearchData.requestType)
        {
            case Search.SceneInteractable.RequestType.Custom:

                GetCustomSceneInteractableData(sceneInteractableSearchData);
                break;

            case Search.SceneInteractable.RequestType.GetQuestAndObjectiveInteractables:

                GetQuestAndObjectiveSceneInteractableData(sceneInteractableSearchData);
                break;

            case Search.SceneInteractable.RequestType.GetInteractablesFromInteractionRegion:

                GetElementsFromInteractionRegion(sceneInteractableSearchData);
                break;
        }

        if (sceneInteractableDataList.Count == 0) return new List<IDataElement>();

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from sceneInteractableData in sceneInteractableDataList
                    join interactableData in interactableDataList on sceneInteractableData.interactableId equals interactableData.id
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new SceneInteractableDataElement()
                    {
                        dataType = Enums.DataType.SceneInteractable,

                        id = sceneInteractableData.id,
                        
                        ChapterId = sceneInteractableData.chapterId,
                        InteractableId = sceneInteractableData.interactableId,

                        interactableName = interactableData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomSceneInteractableData(Search.SceneInteractable searchParameters)
    {
        sceneInteractableDataList = new List<SceneInteractableData>();

        foreach (Fixtures.SceneInteractable sceneInteractable in Fixtures.sceneInteractableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(sceneInteractable.id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(sceneInteractable.chapterId)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(sceneInteractable.objectiveId)) continue;

            var sceneInteractableData = new SceneInteractableData();

            sceneInteractableData.id = sceneInteractable.id;

            sceneInteractableData.chapterId = sceneInteractable.chapterId;
            sceneInteractableData.objectiveId = sceneInteractable.objectiveId;
            sceneInteractableData.interactableId = sceneInteractable.interactableId;

            sceneInteractableDataList.Add(sceneInteractableData);
        }
    }

    internal void GetQuestAndObjectiveSceneInteractableData(Search.SceneInteractable searchParameters)
    {
        sceneInteractableDataList = new List<SceneInteractableData>();
        
        List<int> sceneInteractableIds = new List<int>();

        Fixtures.phaseInteractableList.Where(x => searchParameters.questId.Contains(x.questId)).Distinct().ToList().ForEach(x => sceneInteractableIds.Add(x.sceneInteractableId));
        Fixtures.sceneInteractableList.Where(x => searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList().ForEach(x => sceneInteractableIds.Add(x.id));

        var sceneInteractables = Fixtures.sceneInteractableList.Where(x => sceneInteractableIds.Contains(x.id)).Distinct().ToList();

        foreach (Fixtures.SceneInteractable sceneInteractable in sceneInteractables)
        {
            var sceneInteractableData = new SceneInteractableData();

            sceneInteractableData.id = sceneInteractable.id;

            sceneInteractableData.chapterId = sceneInteractable.chapterId;
            sceneInteractableData.objectiveId = sceneInteractable.objectiveId;
            sceneInteractableData.interactableId = sceneInteractable.interactableId;

            sceneInteractableDataList.Add(sceneInteractableData);
        }
    }

    internal void GetElementsFromInteractionRegion(Search.SceneInteractable searchParameters)
    {
        sceneInteractableDataList = new List<SceneInteractableData>();

        List<int> sceneInteractableIds = Fixtures.interactionList.Where(x => searchParameters.regionId.Contains(x.regionId) &&
                                                                             searchParameters.objectiveId.Contains(x.objectiveId)).Select(x => x.sceneInteractableId).Distinct().ToList();

        var sceneInteractables = Fixtures.sceneInteractableList.Where(x => sceneInteractableIds.Contains(x.id)).Distinct().ToList();

        foreach(Fixtures.SceneInteractable sceneInteractable in sceneInteractables)
        {
            var sceneInteractableData = new SceneInteractableData();

            sceneInteractableData.id = sceneInteractable.id;

            sceneInteractableData.chapterId = sceneInteractable.chapterId;
            sceneInteractableData.objectiveId = sceneInteractable.objectiveId;
            sceneInteractableData.interactableId = sceneInteractable.interactableId;

            sceneInteractableDataList.Add(sceneInteractableData);
        }
    }

    internal void GetInteractableData()
    {
        interactableDataList = dataManager.GetInteractableData(sceneInteractableDataList.Select(x => x.interactableId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class SceneInteractableData : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int interactableId;
        public int interactionIndex;
    }
}
