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
        var list = new List<SceneActorElementData>();

        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneActor>().First();

        GetSceneActorData(searchParameters);

        if (sceneActorDataList.Count > 0)// && !searchParameters.includeEmptyElement) return new List<IElementData>();
        {
            GetWorldInteractableData();
            GetInteractableData();
            GetModelData();
            GetIconData();

            list = (from sceneActorData         in sceneActorDataList
                    join worldInteractableData  in worldInteractableDataList    on sceneActorData.WorldInteractableId   equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData              in modelDataList                on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList                 on modelData.IconId                     equals iconData.Id
                    select new SceneActorElementData()
                    {
                        Id = sceneActorData.Id,

                        SceneId = sceneActorData.SceneId,
                        WorldInteractableId = sceneActorData.WorldInteractableId,

                        SpeechMethod = sceneActorData.SpeechMethod,
                        SpeechText = sceneActorData.SpeechText,
                        ShowTextBox = sceneActorData.ShowTextBox,

                        TargetSceneActorId = sceneActorData.TargetSceneActorId,

                        ChangePosition = sceneActorData.ChangePosition,
                        FreezePosition = sceneActorData.FreezePosition,

                        PositionX = sceneActorData.PositionX,
                        PositionY = sceneActorData.PositionY,
                        PositionZ = sceneActorData.PositionZ,

                        ChangeRotation = sceneActorData.ChangeRotation,
                        FaceTarget = sceneActorData.FaceTarget,

                        RotationX = sceneActorData.RotationX,
                        RotationY = sceneActorData.RotationY,
                        RotationZ = sceneActorData.RotationZ,

                        ModelIconPath = iconData.Path,
                        InteractableName = interactableData.Name,

                        SpeechTextLimit = ScenarioManager.SpeechCharacterLimit(sceneActorData.ShowTextBox)

                    }).OrderBy(x => x.Id).ToList();
        }

        if (searchParameters.includeRemoveElement)
            AddRemoveElementData(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }
    
    private static void GetSceneActorData(Search.SceneActor searchParameters)
    {
        sceneActorDataList = new List<SceneActorBaseData>();
        
        foreach (SceneActorBaseData sceneActor in Fixtures.sceneActorList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(sceneActor.Id))            continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(sceneActor.Id))      continue;
            if (searchParameters.sceneId.Count      > 0 && !searchParameters.sceneId.Contains(sceneActor.SceneId))  continue;

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

    private static void AddRemoveElementData(List<SceneActorElementData> list)
    {
        list.Insert(0, new SceneActorElementData());
    }

    public static void UpdateData(SceneActorElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.sceneActorList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedSpeechMethod)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.SpeechMethod = elementData.SpeechMethod;
            else { }
        }

        if (elementData.ChangedSpeechText)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.SpeechText = elementData.SpeechText;
            else { }
        }

        if (elementData.ChangedShowTextBox)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ShowTextBox = elementData.ShowTextBox;
            else { }
        }

        if (elementData.ChangedTargetSceneActorId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.TargetSceneActorId = elementData.TargetSceneActorId;
            else { }
        }

        if (elementData.ChangedChangePosition)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ChangePosition = elementData.ChangePosition;
            else { }
        }

        if (elementData.ChangedFreezePosition)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.FreezePosition = elementData.FreezePosition;
            else { }
        }

        if (elementData.ChangedPositionX)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionX = elementData.PositionX;
            else { }
        }

        if (elementData.ChangedPositionY)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionY = elementData.PositionY;
            else { }
        }

        if (elementData.ChangedPositionZ)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionZ = elementData.PositionZ;
            else { }
        }

        if (elementData.ChangedChangeRotation)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ChangeRotation = elementData.ChangeRotation;
            else { }
        }

        if (elementData.ChangedFaceTarget)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.FaceTarget = elementData.FaceTarget;
            else { }
        }

        if (elementData.ChangedRotationX)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationX = elementData.RotationX;
            else { }
        }

        if (elementData.ChangedRotationY)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationY = elementData.RotationY;
            else { }
        }

        if (elementData.ChangedRotationZ)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationZ = elementData.RotationZ;
            else { }
        }
    }

    public static void UpdateSearch(SceneActorElementData elementData)
    {
        var data = Fixtures.sceneActorList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedWorldInteractableId)
            data.WorldInteractableId = elementData.WorldInteractableId;
    }
}
