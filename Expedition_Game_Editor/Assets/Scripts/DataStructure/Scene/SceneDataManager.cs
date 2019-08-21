using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneDataManager
{
    private SceneController sceneController;

    private DataManager dataManager = new DataManager();

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;
    private List<DataManager.InteractionData> interactionDataList;
    private List<DataManager.SceneObjectData> sceneObjectDataList;

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

        var list = (from regionData in regionDataList
                    join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.id
                    select new SceneDataElement
                    {
                        id = regionData.id,
                        index = regionData.index,

                        regionSize = regionData.regionSize,
                        terrainSize = regionData.terrainSize,

                        tileSetName = tileSetData.name,
                        tileSize = tileSetData.tileSize,

                        terrainDataList = (from terrainData in terrainDataList
                                           select new SceneDataElement.TerrainData()
                                           {
                                               id = terrainData.id,
                                               index = terrainData.index,

                                               terrainTileDataList = (from terrainTileData in terrainTileDataList
                                                                      where terrainTileData.terrainId == terrainData.id
                                                                      select new SceneDataElement.TerrainData.TerrainTileData()
                                                                      {
                                                                          id = terrainTileData.id,
                                                                          index = terrainTileData.index,

                                                                          tileId = terrainTileData.tileId

                                                                      }).OrderBy(x => x.index).ToList()

                                           }).OrderBy(x => x.index).ToList()
                    });

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetRegionData(Search.Scene searchData)
    {
        var searchParameters = new Search.Region();
        searchParameters.id = searchData.id;

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    private void GetTileSetData()
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
}
