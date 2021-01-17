using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ScenePropDataManager
{
    private static List<ScenePropBaseData> scenePropDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.SceneProp searchParameters)
    {
        GetScenePropData(searchParameters);

        if (searchParameters.includeAddElement)
            scenePropDataList.Add(DefaultData(searchParameters.sceneId.First()));

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

                        Scale = scenePropData.Scale,

                        ModelPath = modelData.Path,
                        ModelIconPath = iconData.Path,

                        ModelName = modelData.Name,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static ScenePropElementData DefaultData(int sceneId)
    {
        var defaultPosition = Vector3.zero;

        if (EditorWorldOrganizer.instance != null)
        {
            defaultPosition = EditorWorldOrganizer.instance.AddElementDefaultPosition();
        }

        return new ScenePropElementData()
        {
            Id = -1,

            ModelId = 1,
            SceneId = sceneId,
            
            Scale = 1,

            PositionX = defaultPosition.x,
            PositionY = defaultPosition.y,
            PositionZ = defaultPosition.z
        };
    }

    public static void SetDefaultAddValues(List<ScenePropElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetScenePropData(Search.SceneProp searchParameters)
    {
        scenePropDataList = new List<ScenePropBaseData>();

        foreach (ScenePropBaseData sceneProp in Fixtures.scenePropList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(sceneProp.Id))             continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(sceneProp.Id))       continue;
            if (searchParameters.sceneId.Count      > 0 && !searchParameters.sceneId.Contains(sceneProp.SceneId))   continue;

            scenePropDataList.Add(sceneProp);
        }
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = scenePropDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(ScenePropElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.scenePropList.Count > 0 ? (Fixtures.scenePropList[Fixtures.scenePropList.Count - 1].Id + 1) : 1;
            Fixtures.scenePropList.Add(((ScenePropData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(ScenePropElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.scenePropList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedModelId)
            {
                data.ModelId = elementData.ModelId;
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

            if (elementData.ChangedScale)
            {
                data.Scale = elementData.Scale;
            }

            elementData.SetOriginalValues();

        } else { }     
    }

    public static void RemoveData(ScenePropElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.scenePropList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
