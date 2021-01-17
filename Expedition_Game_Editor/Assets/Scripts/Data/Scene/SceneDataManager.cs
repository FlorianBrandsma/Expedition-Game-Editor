using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneDataManager : MonoBehaviour
{
    private static List<SceneBaseData> sceneDataList;

    private static List<OutcomeBaseData> outcomeDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(Search.Scene searchParameters)
    {
        GetSceneData(searchParameters);

        if (searchParameters.includeAddElement)
            sceneDataList.Add(DefaultData(searchParameters.outcomeId.First()));

        if (sceneDataList.Count == 0) return new List<IElementData>();
        
        GetOutcomeData();

        GetRegionData();
        GetTerrainData();
        GetTileSetData();
        GetTileData();

        var list = (from sceneData      in sceneDataList
                    join outcomeData    in outcomeDataList  on sceneData.OutcomeId  equals outcomeData.Id

                    join leftJoin in (from regionData   in regionDataList
                                      join tileSetData  in tileSetDataList on regionData.TileSetId equals tileSetData.Id

                                      join leftJoin in (from tileData in tileDataList
                                                        select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                                      select new { regionData, tileSetData, tileData }) on sceneData.RegionId equals leftJoin.regionData.Id into regionData

                    

                    select new SceneElementData()
                    {
                        Id = sceneData.Id,

                        OutcomeId = sceneData.OutcomeId,
                        RegionId = sceneData.RegionId,

                        Index = sceneData.Index,

                        Name = sceneData.Name,

                        FreezeTime = sceneData.FreezeTime,
                        AutoContinue = sceneData.AutoContinue,
                        SetActorsInstantly = sceneData.SetActorsInstantly,

                        SceneDuration = sceneData.SceneDuration,
                        ShotDuration = sceneData.ShotDuration,

                        PublicNotes = sceneData.PublicNotes,
                        PrivateNotes = sceneData.PrivateNotes,

                        InteractionId = outcomeData.InteractionId,

                        RegionName = regionData.FirstOrDefault() != null ? regionData.FirstOrDefault().regionData.Name : "",

                        TileIconPath = regionData.FirstOrDefault() != null ? regionData.FirstOrDefault().tileData.First().tileData.IconPath : ""
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static SceneElementData DefaultData(int outcomeId)
    {
        return new SceneElementData()
        {
            Id = -1,

            OutcomeId = outcomeId,         
            RegionId = -1
        };
    }

    public static void SetDefaultAddValues(List<SceneElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetSceneData(Search.Scene searchParameters)
    {
        sceneDataList = new List<SceneBaseData>();

        foreach (SceneBaseData scene in Fixtures.sceneList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(scene.Id)) continue;
            if (searchParameters.outcomeId.Count    > 0 && !searchParameters.outcomeId.Contains(scene.OutcomeId)) continue;

            sceneDataList.Add(scene);
        }
    }

    private static void GetOutcomeData()
    {
        var searchParameters = new Search.Outcome();
        searchParameters.id = sceneDataList.Select(x => x.OutcomeId).Distinct().ToList();

        outcomeDataList = DataManager.GetOutcomeData(searchParameters);
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = sceneDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
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

    public static void AddData(SceneElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.sceneList.Count > 0 ? (Fixtures.sceneList[Fixtures.sceneList.Count - 1].Id + 1) : 1;
            Fixtures.sceneList.Add(((SceneData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);
            
        } else { }
    }

    private static void AddDependencies(SceneElementData elementData, DataRequest dataRequest)
    {
        if (!dataRequest.includeDependencies) return;

        AddSceneShotData(elementData, dataRequest);
    }

    private static void AddSceneShotData(SceneElementData elementData, DataRequest dataRequest)
    {
        var startSceneShotElementData = SceneShotDataManager.DefaultData(elementData.Id, Enums.SceneShotType.Start);
        startSceneShotElementData.Add(dataRequest);

        var startSceneEndElementData = SceneShotDataManager.DefaultData(elementData.Id, Enums.SceneShotType.End);
        startSceneEndElementData.Add(dataRequest);
    }

    public static void UpdateData(SceneElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.sceneList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedRegionId)
            {
                data.RegionId = elementData.RegionId;
            }

            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedFreezeTime)
            {
                data.FreezeTime = elementData.FreezeTime;
            }

            if (elementData.ChangedAutoContinue)
            {
                data.AutoContinue = elementData.AutoContinue;
            }

            if (elementData.ChangedSetActorsInstantly)
            {
                data.SetActorsInstantly = elementData.SetActorsInstantly;
            }

            if (elementData.ChangedSceneDuration)
            {
                data.SceneDuration = elementData.SceneDuration;
            }

            if (elementData.ChangedShotDuration)
            {
                data.ShotDuration = elementData.ShotDuration;
            }

            if (elementData.ChangedPublicNotes)
            {
                data.PublicNotes = elementData.PublicNotes;
            }

            if (elementData.ChangedPrivateNotes)
            {
                data.PrivateNotes = elementData.PrivateNotes;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void UpdateIndex(SceneElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.sceneList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

        } else { }
    }

    public static void RemoveData(SceneElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.sceneList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

        } else { }
    }

    private static void RemoveDependencies(SceneElementData elementData, DataRequest dataRequest)
    {
        RemoveSceneShotData(elementData, dataRequest);
        RemoveSceneActorData(elementData, dataRequest);
        RemoveScenePropData(elementData, dataRequest);
    }

    private static void RemoveSceneShotData(SceneElementData elementData, DataRequest dataRequest)
    {
        //Scene shot
        var sceneShotSearchParameters = new Search.SceneShot()
        {
            sceneId = new List<int>() { elementData.Id }
        };

        var sceneShotDataList = DataManager.GetSceneShotData(sceneShotSearchParameters);

        sceneShotDataList.ForEach(sceneShotData =>
        {
            var sceneShotElementData = new SceneShotElementData()
            {
                Id = sceneShotData.Id
            };

            sceneShotElementData.Remove(dataRequest);
        });
    }

    private static void RemoveSceneActorData(SceneElementData elementData, DataRequest dataRequest)
    {
        var sceneActorSearchParameters = new Search.SceneActor()
        {
            sceneId = new List<int>() { elementData.Id }
        };

        var sceneActorDataList = DataManager.GetSceneActorData(sceneActorSearchParameters);
        
        sceneActorDataList.ForEach(sceneActorData =>
        {
            var sceneActorElementData = new SceneActorElementData()
            {
                Id = sceneActorData.Id
            };

            sceneActorElementData.Remove(dataRequest);
        });
    }

    private static void RemoveScenePropData(SceneElementData elementData, DataRequest dataRequest)
    {
        var scenePropSearchParameters = new Search.SceneProp()
        {
            sceneId = new List<int>() { elementData.Id }
        };

        var scenePropDataList = DataManager.GetScenePropData(scenePropSearchParameters);

        scenePropDataList.ForEach(scenePropData =>
        {
            var scenePropElementData = new ScenePropElementData()
            {
                Id = scenePropData.Id
            };

            scenePropElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(SceneElementData elementData, DataRequest dataRequest)
    {
        var sceneSearchParameters = new Search.Scene()
        {
            outcomeId = new List<int>() { elementData.OutcomeId }
        };

        var sceneDataList = DataManager.GetSceneData(sceneSearchParameters);

        sceneDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(sceneData =>
        {
            var sceneElementData = new SceneElementData()
            {
                Id = sceneData.Id,
                Index = sceneData.Index
            };

            sceneElementData.SetOriginalValues();

            sceneElementData.Index--;

            sceneElementData.UpdateIndex(dataRequest);
        });
    }
}
