using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ScenePropDataManager
{
    private static List<ScenePropBaseData> scenePropDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneProp>().First();

        GetScenePropData(searchParameters);

        if (scenePropDataList.Count == 0) return new List<IElementData>();

        GetModelData();
        GetIconData();

        var list = (from scenePropData  in scenePropDataList
                    join modelData      in modelDataList    on scenePropData.ModelId    equals modelData.Id
                    join iconData       in iconDataList     on modelData.IconId         equals iconData.Id
                    select new ScenePropElementData()
                    {
                        Id = scenePropData.Id,

                        SceneId = scenePropData.SceneId,
                        ModelId = scenePropData.ModelId,

                        PositionX = scenePropData.PositionX,
                        PositionY = scenePropData.PositionY,
                        PositionZ = scenePropData.PositionZ,

                        RotationX = scenePropData.RotationX,
                        RotationY = scenePropData.RotationY,
                        RotationZ = scenePropData.RotationZ,

                        ModelIconPath = iconData.Path,
                        ModelName = modelData.Name

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetScenePropData(Search.SceneProp searchParameters)
    {
        scenePropDataList = new List<ScenePropBaseData>();

        foreach (ScenePropBaseData sceneProp in Fixtures.scenePropList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(sceneProp.Id))             continue;
            if (searchParameters.sceneId.Count  > 0 && !searchParameters.sceneId.Contains(sceneProp.SceneId))   continue;

            var scenePropData = new ScenePropBaseData();

            scenePropData.Id = sceneProp.Id;

            scenePropData.SceneId = sceneProp.SceneId;
            scenePropData.ModelId = sceneProp.ModelId;

            scenePropData.PositionX = sceneProp.PositionX;
            scenePropData.PositionY = sceneProp.PositionY;
            scenePropData.PositionZ = sceneProp.PositionZ;

            scenePropData.RotationX = sceneProp.RotationX;
            scenePropData.RotationY = sceneProp.RotationY;
            scenePropData.RotationZ = sceneProp.RotationZ;

            scenePropDataList.Add(scenePropData);
        }
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = scenePropDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(ScenePropElementData elementData)
    {
        var data = Fixtures.scenePropList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedModelId)
            data.ModelId = elementData.ModelId;

        if (elementData.ChangedPositionX)
            data.PositionX = elementData.PositionX;

        if (elementData.ChangedPositionY)
            data.PositionY = elementData.PositionY;

        if (elementData.ChangedPositionZ)
            data.PositionZ = elementData.PositionZ;

        if (elementData.ChangedRotationX)
            data.RotationX = elementData.RotationX;

        if (elementData.ChangedRotationY)
            data.RotationY = elementData.RotationY;

        if (elementData.ChangedRotationZ)
            data.RotationZ = elementData.RotationZ;
    }
}
