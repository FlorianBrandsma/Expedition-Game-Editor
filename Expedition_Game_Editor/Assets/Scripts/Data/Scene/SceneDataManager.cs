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

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Scene>().First();

        GetSceneData(searchParameters);

        if (sceneDataList.Count == 0) return new List<IElementData>();
        
        GetOutcomeData();

        GetRegionData();
        GetTerrainData();
        GetTileSetData();
        GetTileData();

        var list = (from sceneData      in sceneDataList
                    join outcomeData    in outcomeDataList  on sceneData.OutcomeId  equals outcomeData.Id

                    join regionData     in regionDataList   on sceneData.RegionId   equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.TileSetId equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

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
                        PhaseId = regionData.PhaseId,

                        RegionName = regionData.Name,

                        TileIconPath = tileData.First().tileData.IconPath
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
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

    public static void UpdateData(SceneElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.sceneList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedRegionId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RegionId = elementData.RegionId;
            else { }
        }

        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }

        if (elementData.ChangedFreezeTime)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.FreezeTime = elementData.FreezeTime;
            else { }
        }

        if (elementData.ChangedAutoContinue)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.AutoContinue = elementData.AutoContinue;
            else { }
        }

        if (elementData.ChangedSetActorsInstantly)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.SetActorsInstantly = elementData.SetActorsInstantly;
            else { }
        }

        if (elementData.ChangedSceneDuration)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.SceneDuration = elementData.SceneDuration;
            else { }
        }

        if (elementData.ChangedShotDuration)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ShotDuration = elementData.ShotDuration;
            else { }
        }

        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PublicNotes = elementData.PublicNotes;
            else { }
        }

        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PrivateNotes = elementData.PrivateNotes;
            else { }
        }
    }

    static public void UpdateIndex(SceneElementData elementData)
    {
        var data = Fixtures.sceneList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
