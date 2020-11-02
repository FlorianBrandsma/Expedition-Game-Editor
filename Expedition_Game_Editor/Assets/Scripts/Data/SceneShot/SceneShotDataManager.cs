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

                        ChangeRotation = sceneShotData.ChangeRotation,

                        RotationX = sceneShotData.RotationX,
                        RotationY = sceneShotData.RotationY,
                        RotationZ = sceneShotData.RotationZ,

                        Description = "Shot " + System.Enum.GetName(typeof(Enums.SceneShotType), sceneShotData.Type).ToLower()

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

            var sceneShotData = new SceneShotBaseData();

            sceneShotData.Id = sceneShot.Id;

            sceneShotData.SceneId = sceneShot.SceneId;
            
            sceneShotData.Type = sceneShot.Type;

            sceneShotData.ChangePosition = sceneShot.ChangePosition;
            
            sceneShotData.PositionX = sceneShot.PositionX;
            sceneShotData.PositionY = sceneShot.PositionY;
            sceneShotData.PositionZ = sceneShot.PositionZ;

            sceneShotData.ChangeRotation = sceneShot.ChangeRotation;

            sceneShotData.RotationX = sceneShot.RotationX;
            sceneShotData.RotationY = sceneShot.RotationY;
            sceneShotData.RotationZ = sceneShot.RotationZ;

            sceneShotDataList.Add(sceneShotData);
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

    public static void UpdateData(SceneShotElementData elementData)
    {
        var data = Fixtures.sceneShotList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedChangePosition)
            data.ChangePosition = elementData.ChangePosition;
        
        if (elementData.ChangedPositionX)
            data.PositionX = elementData.PositionX;

        if (elementData.ChangedPositionY)
            data.PositionY = elementData.PositionY;

        if (elementData.ChangedPositionZ)
            data.PositionZ = elementData.PositionZ;

        if (elementData.ChangedChangeRotation)
            data.ChangeRotation = elementData.ChangeRotation;

        if (elementData.ChangedRotationX)
            data.RotationX = elementData.RotationX;

        if (elementData.ChangedRotationY)
            data.RotationY = elementData.RotationY;

        if (elementData.ChangedRotationZ)
            data.RotationZ = elementData.RotationZ;
    }
}
