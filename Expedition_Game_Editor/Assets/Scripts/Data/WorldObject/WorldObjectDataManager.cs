using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class WorldObjectDataManager
{
    private static List<WorldObjectBaseData> worldObjectDataList;

    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldObject>().First();

        GetWorldObjectData(searchParameters);
        
        if (worldObjectDataList.Count == 0) return new List<IElementData>();

        GetModelData();
        GetIconData();

        var list = (from worldObjectData    in worldObjectDataList
                    join modelData          in modelDataList    on worldObjectData.ModelId  equals modelData.Id
                    join iconData           in iconDataList     on modelData.IconId         equals iconData.Id
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

                        ModelPath = modelData.Path,

                        ModelName = modelData.Name,
                        ModelIconPath = iconData.Path,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
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

    public static void UpdateData(WorldObjectElementData elementData)
    {
        var data = Fixtures.worldObjectList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedModelId)
            data.ModelId = elementData.ModelId;

        if (elementData.ChangedRegionId)
            data.RegionId = elementData.RegionId;
        
        if (elementData.ChangedTerrainId)
            data.TerrainId = elementData.TerrainId;
        
        if (elementData.ChangedTerrainTileId)
            data.TerrainTileId = elementData.TerrainTileId;

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
        
        if (elementData.ChangedScale)
            data.Scale = elementData.Scale;
        
        if (elementData.ChangedAnimation)
            data.Animation = elementData.Animation;
    }

    public static void UpdateSearch(WorldObjectElementData elementData)
    {
        var data = Fixtures.worldObjectList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedModelId)
            data.ModelId = elementData.ModelId;
    }
}
