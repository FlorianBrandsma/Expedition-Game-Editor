using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainObjectDataManager
{
    private TerrainObjectController terrainObjectController;

    private List<TerrainObjectData> terrainObjectDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(TerrainObjectController elementController)
    {
        this.terrainObjectController = elementController;
    }

    public List<IDataElement> GetTerrainObjectDataElements(IEnumerable searchParameters)
    {
        var terrainObjectSearchData = searchParameters.Cast<Search.TerrainObject>().FirstOrDefault();

        switch (terrainObjectSearchData.requestType)
        {
            case Search.TerrainObject.RequestType.Custom:
                GetCustomTerrainObjectData(terrainObjectSearchData);
                break;
        }

        GetObjectGraphicData();
        GetIconData();

        var list = (from terrainObjectData in terrainObjectDataList
                    join objectGraphicData in objectGraphicDataList on terrainObjectData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new TerrainObjectDataElement()
                    {
                        dataType = Enums.DataType.TerrainObject,

                        id = terrainObjectData.id,

                        ObjectGraphicId = terrainObjectData.objectGraphicId,
                        RegionId = terrainObjectData.regionId,

                        BoundToTile = terrainObjectData.boundToTile,

                        PositionX = terrainObjectData.positionX,
                        PositionY = terrainObjectData.positionY,
                        PositionZ = terrainObjectData.positionZ,

                        RotationX = terrainObjectData.rotationX,
                        RotationY = terrainObjectData.rotationY,
                        RotationZ = terrainObjectData.rotationZ,

                        ScaleMultiplier = terrainObjectData.scaleMultiplier,

                        Animation = terrainObjectData.animation,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomTerrainObjectData(Search.TerrainObject searchParameters)
    {
        terrainObjectDataList = new List<TerrainObjectData>();

        foreach (Fixtures.TerrainObject terrainObject in Fixtures.terrainObjectList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrainObject.id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrainObject.regionId)) continue;

            var terrainObjectData = new TerrainObjectData();

            terrainObjectData.id = terrainObject.id;

            terrainObjectData.objectGraphicId = terrainObject.objectGraphicId;
            terrainObjectData.regionId = terrainObject.regionId;

            terrainObjectData.boundToTile = terrainObject.boundToTile;

            terrainObjectData.positionX = terrainObject.positionX;
            terrainObjectData.positionY = terrainObject.positionY;
            terrainObjectData.positionZ = terrainObject.positionZ;

            terrainObjectData.rotationX = terrainObject.rotationX;
            terrainObjectData.rotationY = terrainObject.rotationY;
            terrainObjectData.rotationZ = terrainObject.rotationZ;

            terrainObjectData.scaleMultiplier = terrainObject.scaleMultiplier;

            terrainObjectData.animation = terrainObject.animation;
            
            terrainObjectDataList.Add(terrainObjectData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(terrainObjectDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class TerrainObjectData : GeneralData
    {
        public int objectGraphicId;
        public int regionId;

        public bool boundToTile;

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
