using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class WorldObjectDataManager
{
    private static List<WorldObjectBaseData> worldObjectDataList;

    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.WorldObject searchParameters)
    {
        GetWorldObjectData(searchParameters);

        if (searchParameters.includeAddElement)
            worldObjectDataList.Add(DefaultData(searchParameters.regionId.First()));

        if (worldObjectDataList.Count == 0) return new List<IElementData>();
        
        GetModelData();
        GetIconData();

        var list = (from worldObjectData in worldObjectDataList

                    join leftJoin in (from modelData    in modelDataList
                                      join iconData     in iconDataList on modelData.IconId equals iconData.Id
                                      select new { modelData, iconData }) on worldObjectData.ModelId equals leftJoin.modelData.Id into modelData

                    select new WorldObjectElementData()
                    {
                        Id = worldObjectData.Id,

                        ModelId = worldObjectData.ModelId,
                        RegionId = worldObjectData.RegionId,
                        TerrainId = worldObjectData.TerrainId,
                        TerrainTileId = worldObjectData.TerrainTileId,

                        PositionX = worldObjectData.PositionX,
                        PositionY = worldObjectData.PositionY,
                        PositionZ = worldObjectData.PositionZ,

                        RotationX = worldObjectData.RotationX,
                        RotationY = worldObjectData.RotationY,
                        RotationZ = worldObjectData.RotationZ,

                        Scale = worldObjectData.Scale,

                        Animation = worldObjectData.Animation,

                        ModelPath = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Path : "",

                        ModelName = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Name : "",
                        ModelIconPath = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().iconData.Path : "",

                        Height = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Height : 0,
                        Width = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Width : 0,
                        Depth = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Depth : 0

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static WorldObjectElementData DefaultData(int regionId)
    {
        var defaultPosition = Vector3.zero;

        if (EditorWorldOrganizer.instance != null)
        {
            defaultPosition = EditorWorldOrganizer.instance.AddElementDefaultPosition();
        }

        return new WorldObjectElementData()
        {
            Id = -1,

            ModelId = 1,
            RegionId = regionId,

            Scale = 1,

            PositionX = defaultPosition.x,
            PositionY = defaultPosition.y,
            PositionZ = defaultPosition.z
        };
    }

    public static void SetDefaultAddValues(List<WorldObjectElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetWorldObjectData(Search.WorldObject searchParameters)
    {
        worldObjectDataList = new List<WorldObjectBaseData>();

        foreach (WorldObjectBaseData worldObject in Fixtures.worldObjectList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(worldObject.Id))               continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(worldObject.RegionId))   continue;

            worldObjectDataList.Add(worldObject);
        }
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = worldObjectDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(WorldObjectElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.worldObjectList.Count > 0 ? (Fixtures.worldObjectList[Fixtures.worldObjectList.Count - 1].Id + 1) : 1;
            Fixtures.worldObjectList.Add(((WorldObjectData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(WorldObjectElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.worldObjectList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedModelId)
            {
                data.ModelId = elementData.ModelId;
            }

            if (elementData.ChangedRegionId)
            {
                data.RegionId = elementData.RegionId;
            }

            if (elementData.ChangedTerrainId)
            {
                data.TerrainId = elementData.TerrainId;
            }

            if (elementData.ChangedTerrainTileId)
            {
                data.TerrainTileId = elementData.TerrainTileId;
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

            if (elementData.ChangedAnimation)
            {
                data.Animation = elementData.Animation;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(WorldObjectElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.worldObjectList.RemoveAll(x => x.Id == elementData.Id);
        } else { }
    }
}
