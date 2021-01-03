using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SceneShotDataManager
{
    private static List<SceneShotBaseData> sceneShotDataList;

    public static List<IElementData> GetData(Search.SceneShot searchParameters)
    {
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

    public static void AddData(SceneShotElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.sceneShotList.Count > 0 ? (Fixtures.sceneShotList[Fixtures.sceneShotList.Count - 1].Id + 1) : 1;
            Fixtures.sceneShotList.Add(((SceneShotData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(SceneShotElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.sceneShotList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedChangePosition)
            {
                data.ChangePosition = elementData.ChangePosition;
            }

            if (elementData.ChangedPositionX)
            {
                data.PositionX = elementData.PositionX;
            }

            if (elementData.ChangedPositionY)
            {
                data.PositionY = elementData.PositionY;
            }

            if (elementData.ChangedPositionZ)
            {
                data.PositionZ = elementData.PositionZ;
            }

            if (elementData.ChangedPositionTargetSceneActorId)
            {
                data.PositionTargetSceneActorId = elementData.PositionTargetSceneActorId;
            }

            if (elementData.ChangedChangeRotation)
            {
                data.ChangeRotation = elementData.ChangeRotation;
            }

            if (elementData.ChangedRotationX)
            {
                data.RotationX = elementData.RotationX;
            }

            if (elementData.ChangedRotationY)
            {
                data.RotationY = elementData.RotationY;
            }

            if (elementData.ChangedRotationZ)
            {
                data.RotationZ = elementData.RotationZ;
            }

            if (elementData.ChangedRotationTargetSceneActorId)
            {
                data.RotationTargetSceneActorId = elementData.RotationTargetSceneActorId;
            }

            if (elementData.ChangedCameraFilterId)
            {
                data.CameraFilterId = elementData.CameraFilterId;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(SceneShotElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.sceneShotList.RemoveAll(x => x.Id == elementData.Id);
        }
    }
}
