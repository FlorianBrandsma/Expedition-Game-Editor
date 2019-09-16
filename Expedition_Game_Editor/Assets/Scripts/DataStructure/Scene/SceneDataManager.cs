using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneDataManager
{
    private SceneController sceneController;

    private SceneDataElement basicSceneData;

    private DataManager dataManager = new DataManager();

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;
    private List<DataManager.InteractionData> interactionDataList;
    private List<DataManager.SceneObjectData> sceneObjectDataList;
    private List<DataManager.SceneInteractableData> sceneInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;

    public void InitializeManager(SceneController sceneController)
    {
        this.sceneController = sceneController;
    }

    public List<IDataElement> GetSceneDataElements(IEnumerable searchParameters)
    {
        var searchData = searchParameters.Cast<Search.Scene>().FirstOrDefault();

        GetRegionData(searchData);
        GetTileSetData();
        GetTerrainData();
        GetTerrainTileData();        
        GetInteractionData(searchData);
        GetSceneObjectData(searchData);
        
        GetSceneInteractableData();
        GetInteractableData();
        GetObjectGraphicData();

        basicSceneData = GetBasicSceneData();

        var sceneStartPosition = GetSceneStartPosition();
        
        var list = (from regionData in regionDataList
                    join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.id
                    select new SceneDataElement
                    {
                        dataType = Enums.DataType.Scene,

                        id = regionData.id,
                        index = regionData.index,

                        regionSize = regionData.regionSize,
                        terrainSize = regionData.terrainSize,

                        tileSetName = tileSetData.name,
                        tileSize = tileSetData.tileSize,

                        startPosition = sceneStartPosition,

                        terrainDataList = (from terrainData in terrainDataList
                                           select new SceneDataElement.TerrainData()
                                           {
                                               dataType = Enums.DataType.Terrain,

                                               id = terrainData.id,
                                               index = terrainData.index,

                                               terrainTileDataList = (from terrainTileData in terrainTileDataList
                                                                      where terrainTileData.terrainId == terrainData.id
                                                                      select new TerrainTileDataElement()
                                                                      {
                                                                          dataType = Enums.DataType.TerrainTile,

                                                                          id = terrainTileData.id,
                                                                          index = terrainTileData.index,

                                                                          TileId = terrainTileData.tileId

                                                                      }).OrderBy(x => x.index).ToList(),

                                               interactionDataList = (from interactionData          in interactionDataList
                                                                      join sceneInteractableData    in sceneInteractableDataList    on interactionData.sceneInteractableId  equals sceneInteractableData.id
                                                                      join interactableData         in interactableDataList         on sceneInteractableData.interactableId equals interactableData.id
                                                                      join objectGraphicData        in objectGraphicDataList        on interactableData.objectGraphicId     equals objectGraphicData.id
                                                                      where interactionData.terrainId == terrainData.id
                                                                      select new InteractionDataElement()
                                                                      {
                                                                          dataType = Enums.DataType.Interaction,

                                                                          id = interactionData.id,
                                                                          SceneInteractableId = interactionData.sceneInteractableId,
                                                                          TerrainTileId = interactionData.terrainTileId,

                                                                          PositionX = interactionData.positionX,
                                                                          PositionY = interactionData.positionY,
                                                                          PositionZ = interactionData.positionZ,

                                                                          RotationX = interactionData.rotationX,
                                                                          RotationY = interactionData.rotationY,
                                                                          RotationZ = interactionData.rotationZ,

                                                                          ScaleMultiplier = interactionData.scaleMultiplier,

                                                                          Animation = interactionData.animation,

                                                                          objectGraphicId = objectGraphicData.id,
                                                                          objectGraphicPath = objectGraphicData.path,

                                                                          startPosition = sceneStartPosition

                                                                      }).ToList(),

                                               sceneObjectDataList = (from sceneObjectData          in sceneObjectDataList
                                                                      join objectGraphicData        in objectGraphicDataList        on sceneObjectData.objectGraphicId      equals objectGraphicData.id
                                                                      where sceneObjectData.terrainId == terrainData.id
                                                                      select new SceneObjectDataElement()
                                                                      {
                                                                          dataType = Enums.DataType.SceneObject,

                                                                          id = sceneObjectData.id,
                                                                          TerrainTileId = sceneObjectData.terrainTileId,

                                                                          PositionX = sceneObjectData.positionX,
                                                                          PositionY = sceneObjectData.positionY,
                                                                          PositionZ = sceneObjectData.positionZ,

                                                                          RotationX = sceneObjectData.rotationX,
                                                                          RotationY = sceneObjectData.rotationY,
                                                                          RotationZ = sceneObjectData.rotationZ,

                                                                          ScaleMultiplier = sceneObjectData.scaleMultiplier,

                                                                          Animation = sceneObjectData.animation,

                                                                          ObjectGraphicId = objectGraphicData.id,
                                                                          objectGraphicPath = objectGraphicData.path,

                                                                          startPosition = sceneStartPosition

                                                                      }).ToList()

                                           }).OrderBy(x => x.index).ToList()
                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    private SceneDataElement GetBasicSceneData()
    {
        var sceneData = (from regionData in regionDataList
                         join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.id
                         select new SceneDataElement
                         {
                             regionSize = regionData.regionSize,
                             terrainSize = regionData.terrainSize,
                             tileSize = tileSetData.tileSize

                         }).FirstOrDefault();

        return sceneData;
    }

    private Vector2 GetSceneStartPosition()
    {
        var sceneStartPosition = new Vector2(-(basicSceneData.regionSize * basicSceneData.terrainSize * basicSceneData.tileSize / 2),
                                              (basicSceneData.regionSize * basicSceneData.terrainSize * basicSceneData.tileSize / 2));

        return sceneStartPosition;
    }

    internal void GetRegionData(Search.Scene searchData)
    {
        var searchParameters = new Search.Region();
        searchParameters.id = searchData.id;

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    internal void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.regionId = regionDataList.Select(x => x.id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(terrainSearchParameters);
    }

    internal void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.terrainId = terrainDataList.Select(x => x.id).Distinct().ToList();

        terrainTileDataList = dataManager.GetTerrainTileData(terrainTileSearchParameters);
    }

    internal void GetInteractionData(Search.Scene searchData)
    {
        var interactionSearchParameters = new Search.Interaction();
        interactionSearchParameters.regionId = searchData.id;

        interactionDataList = dataManager.GetInteractionData(interactionSearchParameters);
    }

    internal void GetSceneObjectData(Search.Scene searchData)
    {
        var sceneObjectSearchParameters = new Search.SceneObject();
        sceneObjectSearchParameters.regionId = searchData.id;

        sceneObjectDataList = dataManager.GetSceneObjectData(sceneObjectSearchParameters);
    }

    internal void GetSceneInteractableData()
    {
        var sceneInteractableSearchParameters = new Search.SceneInteractable();

        sceneInteractableSearchParameters.id = interactionDataList.Select(x => x.sceneInteractableId).Distinct().ToList();

        sceneInteractableDataList = dataManager.GetSceneInteractableData(sceneInteractableSearchParameters);
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = sceneInteractableDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList().Union(sceneObjectDataList.Select(x => x.objectGraphicId).Distinct().ToList()).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }
}
