using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<WorldObjectData> worldObjectDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public WorldObjectDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var worldObjectSearchData = searchParameters.Cast<Search.WorldObject>().FirstOrDefault();

        switch (worldObjectSearchData.requestType)
        {
            case Search.WorldObject.RequestType.Custom:
                GetCustomWorldObjectData(worldObjectSearchData);
                break;
        }

        GetObjectGraphicData();
        GetIconData();

        var list = (from worldObjectData    in worldObjectDataList
                    join objectGraphicData  in objectGraphicDataList    on worldObjectData.objectGraphicId  equals objectGraphicData.Id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.Id
                    select new WorldObjectDataElement()
                    {
                        Id = worldObjectData.Id,
                        Index = worldObjectData.Index,

                        ObjectGraphicId = worldObjectData.objectGraphicId,
                        RegionId = worldObjectData.regionId,
                        TerrainId = worldObjectData.terrainId,
                        TerrainTileId = worldObjectData.terrainTileId,

                        PositionX = worldObjectData.positionX,
                        PositionY = worldObjectData.positionY,
                        PositionZ = worldObjectData.positionZ,

                        RotationX = worldObjectData.rotationX,
                        RotationY = worldObjectData.rotationY,
                        RotationZ = worldObjectData.rotationZ,

                        ScaleMultiplier = worldObjectData.scaleMultiplier,

                        Animation = worldObjectData.animation,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIconPath = iconData.path,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomWorldObjectData(Search.WorldObject searchParameters)
    {
        worldObjectDataList = new List<WorldObjectData>();

        foreach (Fixtures.WorldObject worldObject in Fixtures.worldObjectList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(worldObject.Id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(worldObject.regionId)) continue;

            var worldObjectData = new WorldObjectData();

            worldObjectData.Id = worldObject.Id;
            worldObjectData.Index = worldObject.Index;

            worldObjectData.objectGraphicId = worldObject.objectGraphicId;
            worldObjectData.regionId = worldObject.regionId;
            worldObjectData.terrainId = worldObject.terrainId;
            worldObjectData.terrainTileId = worldObject.terrainTileId;

            worldObjectData.positionX = worldObject.positionX;
            worldObjectData.positionY = worldObject.positionY;
            worldObjectData.positionZ = worldObject.positionZ;

            worldObjectData.rotationX = worldObject.rotationX;
            worldObjectData.rotationY = worldObject.rotationY;
            worldObjectData.rotationZ = worldObject.rotationZ;

            worldObjectData.scaleMultiplier = worldObject.scaleMultiplier;

            worldObjectData.animation = worldObject.animation;
            
            worldObjectDataList.Add(worldObjectData);
        }
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = worldObjectDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal class WorldObjectData : GeneralData
    {
        public int objectGraphicId;
        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }
}
