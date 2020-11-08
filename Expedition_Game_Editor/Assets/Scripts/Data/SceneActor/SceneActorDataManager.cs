using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SceneActorDataManager
{
    private static List<SceneActorBaseData> sceneActorDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneActor>().First();

        GetSceneActorData(searchParameters);

        if (sceneActorDataList.Count == 0) return new List<IElementData>();

        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from sceneShotData          in sceneActorDataList
                    join worldInteractableData  in worldInteractableDataList    on sceneShotData.WorldInteractableId    equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData              in modelDataList                on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList                 on modelData.IconId                     equals iconData.Id
                    select new SceneActorElementData()
                    {
                        Id = sceneShotData.Id,

                        SceneId = sceneShotData.SceneId,
                        WorldInteractableId = sceneShotData.WorldInteractableId,

                        SpeechMethod = sceneShotData.SpeechMethod,
                        SpeechText = sceneShotData.SpeechText,
                        ShowTextBox = sceneShotData.ShowTextBox,

                        ChangePosition = sceneShotData.ChangePosition,
                        FreezePosition = sceneShotData.FreezePosition,

                        PositionX = sceneShotData.PositionX,
                        PositionY = sceneShotData.PositionY,
                        PositionZ = sceneShotData.PositionZ,

                        ChangeRotation = sceneShotData.ChangeRotation,
                        FaceTarget = sceneShotData.FaceTarget,

                        RotationX = sceneShotData.RotationX,
                        RotationY = sceneShotData.RotationY,
                        RotationZ = sceneShotData.RotationZ,

                        ModelIconPath = iconData.Path,
                        InteractableName = interactableData.Name

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetSceneActorData(Search.SceneActor searchParameters)
    {
        sceneActorDataList = new List<SceneActorBaseData>();

        foreach (SceneActorBaseData sceneActor in Fixtures.sceneActorList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(sceneActor.Id)) continue;
            if (searchParameters.sceneId.Count  > 0 && !searchParameters.sceneId.Contains(sceneActor.SceneId)) continue;

            var sceneActorData = new SceneActorBaseData();

            sceneActorData.Id = sceneActor.Id;

            sceneActorData.SceneId = sceneActor.SceneId;
            sceneActorData.WorldInteractableId = sceneActor.WorldInteractableId;

            sceneActorData.SpeechMethod = sceneActor.SpeechMethod;
            sceneActorData.SpeechText = sceneActor.SpeechText;
            sceneActorData.ShowTextBox = sceneActor.ShowTextBox;

            sceneActorData.ChangePosition = sceneActor.ChangePosition;
            sceneActorData.FreezePosition = sceneActor.FreezePosition;

            sceneActorData.PositionX = sceneActor.PositionX;
            sceneActorData.PositionY = sceneActor.PositionY;
            sceneActorData.PositionZ = sceneActor.PositionZ;

            sceneActorData.ChangeRotation = sceneActor.ChangeRotation;
            sceneActorData.FaceTarget = sceneActor.FaceTarget;

            sceneActorData.RotationX = sceneActor.RotationX;
            sceneActorData.RotationY = sceneActor.RotationY;
            sceneActorData.RotationZ = sceneActor.RotationZ;

            sceneActorDataList.Add(sceneActorData);
        }
    }

    private static void GetWorldInteractableData()
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable();

        worldInteractableSearchParameters.id = sceneActorDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(SceneActorElementData elementData)
    {
        var data = Fixtures.sceneActorList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedWorldInteractableId)
            data.WorldInteractableId = elementData.WorldInteractableId;

        if (elementData.ChangedSpeechMethod)
            data.SpeechMethod = elementData.SpeechMethod;

        if (elementData.ChangedSpeechText)
            data.SpeechText = elementData.SpeechText;

        if (elementData.ChangedShowTextBox)
            data.ShowTextBox = elementData.ShowTextBox;

        if (elementData.ChangedChangePosition)
            data.ChangePosition = elementData.ChangePosition;

        if (elementData.ChangedFreezePosition)
            data.FreezePosition = elementData.FreezePosition;

        if (elementData.ChangedPositionX)
            data.PositionX = elementData.PositionX;

        if (elementData.ChangedPositionY)
            data.PositionY = elementData.PositionY;

        if (elementData.ChangedPositionZ)
            data.PositionZ = elementData.PositionZ;

        if (elementData.ChangedChangeRotation)
            data.ChangeRotation = elementData.ChangeRotation;

        if (elementData.ChangedFaceTarget)
            data.FaceTarget = elementData.FaceTarget;

        if (elementData.ChangedRotationX)
            data.RotationX = elementData.RotationX;

        if (elementData.ChangedRotationY)
            data.RotationY = elementData.RotationY;

        if (elementData.ChangedRotationZ)
            data.RotationZ = elementData.RotationZ;
    }
}
