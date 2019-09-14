using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectDataManager
{
    private SceneObjectController sceneObjectController;

    private List<SceneObjectData> sceneObjectDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(SceneObjectController elementController)
    {
        this.sceneObjectController = elementController;
    }

    public List<IDataElement> GetSceneObjectDataElements(IEnumerable searchParameters)
    {
        var sceneObjectSearchData = searchParameters.Cast<Search.SceneObject>().FirstOrDefault();

        switch (sceneObjectSearchData.requestType)
        {
            case Search.SceneObject.RequestType.Custom:
                GetCustomSceneObjectData(sceneObjectSearchData);
                break;
        }

        GetObjectGraphicData();
        GetIconData();

        var list = (from sceneObjectData in sceneObjectDataList
                    join objectGraphicData in objectGraphicDataList on sceneObjectData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new SceneObjectDataElement()
                    {
                        dataType = Enums.DataType.SceneObject,

                        id = sceneObjectData.id,

                        ObjectGraphicId = sceneObjectData.objectGraphicId,
                        RegionId = sceneObjectData.regionId,
                        TerrainId = sceneObjectData.terrainId,
                        TerrainTileId = sceneObjectData.terrainTileId,

                        PositionX = sceneObjectData.positionX,
                        PositionY = sceneObjectData.positionY,
                        PositionZ = sceneObjectData.positionZ,

                        RotationX = sceneObjectData.rotationX,
                        RotationY = sceneObjectData.rotationY,
                        RotationZ = sceneObjectData.rotationZ,

                        ScaleMultiplier = sceneObjectData.scaleMultiplier,

                        Animation = sceneObjectData.animation,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomSceneObjectData(Search.SceneObject searchParameters)
    {
        sceneObjectDataList = new List<SceneObjectData>();

        foreach (Fixtures.SceneObject sceneObject in Fixtures.sceneObjectList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(sceneObject.id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(sceneObject.regionId)) continue;

            var sceneObjectData = new SceneObjectData();

            sceneObjectData.id = sceneObject.id;

            sceneObjectData.objectGraphicId = sceneObject.objectGraphicId;
            sceneObjectData.regionId = sceneObject.regionId;
            sceneObjectData.terrainId = sceneObject.terrainId;
            sceneObjectData.terrainTileId = sceneObject.terrainTileId;

            sceneObjectData.positionX = sceneObject.positionX;
            sceneObjectData.positionY = sceneObject.positionY;
            sceneObjectData.positionZ = sceneObject.positionZ;

            sceneObjectData.rotationX = sceneObject.rotationX;
            sceneObjectData.rotationY = sceneObject.rotationY;
            sceneObjectData.rotationZ = sceneObject.rotationZ;

            sceneObjectData.scaleMultiplier = sceneObject.scaleMultiplier;

            sceneObjectData.animation = sceneObject.animation;
            
            sceneObjectDataList.Add(sceneObjectData);
        }
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = sceneObjectDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class SceneObjectData : GeneralData
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
