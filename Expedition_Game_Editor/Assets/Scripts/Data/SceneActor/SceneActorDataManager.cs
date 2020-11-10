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

            sceneActorDataList.Add(sceneActor);
        }
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = sceneActorDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void UpdateData(SceneActorElementData elementData)
    {
        var data = Fixtures.sceneActorList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
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

    public static void UpdateSearch(SceneActorElementData elementData)
    {
        var data = Fixtures.sceneActorList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedWorldInteractableId)
            data.WorldInteractableId = elementData.WorldInteractableId;
    }
}
