using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class RegionDataManager
{
    private static List<RegionBaseData> regionDataList;

    private static List<TerrainBaseData> terrainDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(Search.Region searchParameters)
    {
        GetRegionData(searchParameters);

        if (searchParameters.includeAddElement)
            regionDataList.Add(DefaultData());

        if (searchParameters.includeRemoveElement)
            regionDataList.Add(new RegionBaseData());

        if (regionDataList.Count == 0) return new List<IElementData>();
        
        GetTerrainData();
        GetTerrainTileData();

        GetTileSetData();
        GetTileData();

        var list = (from regionData in regionDataList

                    join leftJoin in (from tileSetData in tileSetDataList

                                      join leftJoin in (from tileData in tileDataList
                                                        select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                                      select new { tileSetData, tileData }) on regionData.TileSetId equals leftJoin.tileSetData.Id into tileSetData

                    
                    select new RegionElementData()
                    {
                        Id = regionData.Id,
                        
                        ChapterRegionId = regionData.ChapterRegionId,
                        PhaseId = regionData.PhaseId,
                        TileSetId = regionData.TileSetId,

                        Index = regionData.Index,

                        Name = regionData.Name,

                        RegionSize = regionData.RegionSize,
                        TerrainSize = regionData.TerrainSize,

                        Type = searchParameters.type,

                        TileSize = tileSetData.FirstOrDefault() != null ? tileSetData.FirstOrDefault().tileSetData.TileSize : 0,
                        TileIconPath = tileSetData.FirstOrDefault() != null ? tileSetData.FirstOrDefault().tileData.First().tileData.IconPath : "",

                        TerrainDataList = 
                        (from terrainData in terrainDataList where regionData.Id == terrainData.RegionId
                         select new TerrainElementData()
                         {
                             Id = terrainData.Id,

                             RegionId = terrainData.RegionId,

                             Index = terrainData.Index,
                             
                             TerrainTileDataList =
                             (from terrainTileData in terrainTileDataList where terrainData.Id == terrainTileData.TerrainId
                              select new TerrainTileElementData()
                              {
                                  Id = terrainTileData.Id,

                                  TerrainId = terrainTileData.TerrainId,

                                  Index = terrainTileData.Index

                              }).ToList()
                         }).ToList()
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static RegionElementData DefaultData()
    {
        return new RegionElementData()
        {
            Id = -1,

            TileSetId = 1,
            RegionSize = 1,
            TerrainSize = 1
        };
    }

    public static void SetDefaultAddValues(List<RegionElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetRegionData(Search.Region searchParameters)
    {
        regionDataList = new List<RegionBaseData>();

        foreach(RegionBaseData region in Fixtures.regionList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(region.Id))                    continue;
            if (searchParameters.excludeId.Count        > 0 && searchParameters.excludeId.Contains(region.Id))              continue;
            if (searchParameters.projectId.Count        > 0 && !searchParameters.projectId.Contains(region.ProjectId))      continue;
            if (searchParameters.phaseId.Count          > 0 && !searchParameters.phaseId.Contains(region.PhaseId))          continue;
            if (searchParameters.excludePhaseId.Count   > 0 && searchParameters.excludePhaseId.Contains(region.PhaseId))    continue;

            regionDataList.Add(region);
        }
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();
        searchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    private static void GetTileData()
    {
        var searchParameters = new Search.Tile();
        searchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = DataManager.GetTileData(searchParameters);
    }

    public static void AddData(RegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.regionList.Count > 0 ? (Fixtures.regionList[Fixtures.regionList.Count - 1].Id + 1) : 1;
            Fixtures.regionList.Add(((RegionData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);
            
        } else { }
    }

    private static void AddDependencies(RegionElementData elementData, DataRequest dataRequest)
    {
        if (!dataRequest.includeDependencies) return;

        AddTerrainData(elementData, dataRequest);
    }

    private static void AddTerrainData(RegionElementData elementData, DataRequest dataRequest)
    {
        var tileSearchParameters = new Search.Tile()
        {
            tileSetId = new List<int>() { elementData.TileSetId }
        };

        var tileData = DataManager.GetTileData(tileSearchParameters);

        for (int regionIndex = 0; regionIndex < elementData.RegionSize * elementData.RegionSize; regionIndex++)
        {
            var terrainElementData = TerrainDataManager.DefaultData(elementData.Id, regionIndex);
            terrainElementData.Add(dataRequest);

            for (int terrainIndex = 0; terrainIndex < elementData.TerrainSize * elementData.TerrainSize; terrainIndex++)
            {
                var terrainTileElementData = TerrainTileDataManager.DefaultData(terrainElementData.Id, tileData.First().Id, terrainIndex);
                terrainTileElementData.Add(dataRequest);
            }
        }
    }

    public static void UpdateData(RegionElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedChapterRegionId)
            {
                data.ChapterRegionId = elementData.ChapterRegionId;
            }

            if (elementData.ChangedPhaseId)
            {
                data.PhaseId = elementData.PhaseId;
            }

            if (elementData.ChangedTileSetId)
            {
                data.TileSetId = elementData.TileSetId;
            }

            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedRegionSize)
            {
                data.RegionSize = elementData.RegionSize;
            }

            if (elementData.ChangedTerrainSize)
            {
                data.TerrainSize = elementData.TerrainSize;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void UpdateIndex(RegionElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

        } else { }
    }

    public static void RemoveData(RegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.regionList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(RegionElementData elementData, DataRequest dataRequest)
    {
        RemoveChapterRegionData(elementData, dataRequest);

        RemoveTerrainData(elementData, dataRequest);
        RemoveWorldObjectData(elementData, dataRequest);
        RemoveWorldInteractableData(elementData, dataRequest);
        RemoveInteractionDestinationData(elementData, dataRequest);
    }

    private static void RemoveChapterRegionData(RegionElementData elementData, DataRequest dataRequest)
    {
        var chapterRegionSearchParameters = new Search.ChapterRegion()
        {
            regionId = new List<int>() { elementData.Id }
        };

        var chapterRegionDataList = DataManager.GetChapterRegionData(chapterRegionSearchParameters);

        chapterRegionDataList.ForEach(chapterRegionData =>
        {
            var chapterRegionElementData = new ChapterRegionElementData()
            {
                Id = chapterRegionData.Id
            };

            chapterRegionElementData.Remove(dataRequest);
        });
    }

    private static void RemoveTerrainData(RegionElementData elementData, DataRequest dataRequest)
    {
        var terrainSearchParameters = new Search.Terrain()
        {
            regionId = new List<int>() { elementData.Id }
        };

        var terrainDataList = DataManager.GetTerrainData(terrainSearchParameters);

        terrainDataList.ForEach(terrainData =>
        {
            var terrainElementData = new TerrainElementData()
            {
                Id = terrainData.Id
            };

            terrainElementData.Remove(dataRequest);
        });
    }

    private static void RemoveWorldObjectData(RegionElementData elementData, DataRequest dataRequest)
    {
        var worldObjectSearchParameters = new Search.WorldObject()
        {
            regionId = new List<int>() { elementData.Id }
        };

        var worldObjectDataList = DataManager.GetWorldObjectData(worldObjectSearchParameters);

        worldObjectDataList.ForEach(worldObjectData =>
        {
            var worldObjectElementData = new WorldObjectElementData()
            {
                Id = worldObjectData.Id
            };

            worldObjectElementData.Remove(dataRequest);
        });
    }

    private static void RemoveWorldInteractableData(RegionElementData elementData, DataRequest dataRequest)
    {
        //Interaction destination
        var interactionDestinationSearchParameters = new Search.InteractionDestination()
        {
            regionId = new List<int>() { elementData.Id }
        };

        var interactionDestinationDataList = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);

        if (interactionDestinationDataList.Count == 0) return;

        //Interaction
        var interactionSearchParameters = new Search.Interaction()
        {
            id = interactionDestinationDataList.Select(x => x.InteractionId).ToList()
        };

        var interactionDataList = DataManager.GetInteractionData(interactionSearchParameters);

        //Task
        var taskSearchParameters = new Search.Task()
        {
            id = interactionDataList.Select(x => x.TaskId).ToList()
        };

        var taskDataList = DataManager.GetTaskData(taskSearchParameters);

        //World interactable

        //Only remove the world interactables which belong directly to the region
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            id = taskDataList.Select(x => x.WorldInteractableId).ToList(),
            phaseId = new List<int>() { 0 },
            objectiveId = new List<int>() { 0 }
        };

        var worldInteractableDataSourceList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);
        
        worldInteractableDataSourceList.ForEach(worldInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id
            };

            worldInteractableElementData.Remove(dataRequest);
        });
    }

    private static void RemoveInteractionDestinationData(RegionElementData elementData, DataRequest dataRequest)
    {
        //Interaction destination
        var interactionDestinationSearchParameters = new Search.InteractionDestination()
        {
            regionId = new List<int>() { elementData.Id }
        };

        var interactionDestinationDataList = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);

        interactionDestinationDataList.ForEach(interactionDestinationData =>
        {
            var interactionDestinationElementData = new InteractionDestinationElementData()
            {
                Id = interactionDestinationData.Id
            };

            interactionDestinationElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(RegionElementData elementData, DataRequest dataRequest)
    {
        var regionSearchParameters = new Search.Region()
        {
            phaseId = new List<int>() { 0 }
        };

        var regionDataList = DataManager.GetRegionData(regionSearchParameters);

        regionDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(regionData =>
        {
            var regionElementData = new RegionElementData()
            {
                Id = regionData.Id,
                Index = regionData.Index
            };

            regionElementData.SetOriginalValues();

            regionElementData.Index--;

            regionElementData.UpdateIndex(dataRequest);
        });
    }
}
