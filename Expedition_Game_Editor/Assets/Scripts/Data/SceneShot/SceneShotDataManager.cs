using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SceneShotDataManager
{
    private static List<SceneShotBaseData> sceneShotDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneShot>().First();

        GetSceneShotData(searchParameters);

        if (sceneShotDataList.Count == 0) return new List<IElementData>();
        
        sceneShotDataList.Select(x => x.SceneId).Distinct().ToList().ForEach(x => AddDefaultShot(x));

        var list = (from sceneShotData in sceneShotDataList
                    select new SceneShotElementData()
                    {
                        Id = sceneShotData.Id,

                        SceneId = sceneShotData.SceneId,

                        Type = sceneShotData.Type,

                        ChangePosition = sceneShotData.ChangePosition,

                        PositionX = sceneShotData.PositionX,
                        PositionY = sceneShotData.PositionY,
                        PositionZ = sceneShotData.PositionZ,

                        PositionTargetSceneActorId = sceneShotData.PositionTargetSceneActorId,

                        ChangeRotation = sceneShotData.ChangeRotation,

                        RotationX = sceneShotData.RotationX,
                        RotationY = sceneShotData.RotationY,
                        RotationZ = sceneShotData.RotationZ,

                        RotationTargetSceneActorId = sceneShotData.RotationTargetSceneActorId,

                        CameraFilterId = sceneShotData.CameraFilterId

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetSceneShotData(Search.SceneShot searchParameters)
    {
        sceneShotDataList = new List<SceneShotBaseData>();

        foreach (SceneShotBaseData sceneShot in Fixtures.sceneShotList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(sceneShot.Id))             continue;
            if (searchParameters.sceneId.Count  > 0 && !searchParameters.sceneId.Contains(sceneShot.SceneId))   continue;

            sceneShotDataList.Add(sceneShot);
        }
    }

    private static void AddDefaultShot(int sceneId)
    {
        var sceneShotData = new SceneShotBaseData()
        {
            SceneId = sceneId
        };

        sceneShotDataList.Add(sceneShotData);
    }

    public static void UpdateData(SceneShotElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.sceneShotList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedChangePosition)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ChangePosition = elementData.ChangePosition;
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

        if (elementData.ChangedPositionTargetSceneActorId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionTargetSceneActorId = elementData.PositionTargetSceneActorId;
            else { }
        }

        if (elementData.ChangedChangeRotation)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ChangeRotation = elementData.ChangeRotation;
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

        if (elementData.ChangedRotationTargetSceneActorId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationTargetSceneActorId = elementData.RotationTargetSceneActorId;
            else { }
        }

        if (elementData.ChangedCameraFilterId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.CameraFilterId = elementData.CameraFilterId;
            else { }
        }
    }
}
