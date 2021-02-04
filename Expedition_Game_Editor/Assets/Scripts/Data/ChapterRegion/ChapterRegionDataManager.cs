using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterRegionDataManager
{
    private static List<ChapterRegionBaseData> chapterRegionDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(Search.ChapterRegion searchParameters)
    {
        GetChapterRegionData(searchParameters);

        if (searchParameters.includeAddElement)
            chapterRegionDataList.Add(DefaultData());

        if (chapterRegionDataList.Count == 0) return new List<IElementData>();
        
        GetRegionData();
        GetTileSetData();
        GetTileData();

        var list = (from chapterRegionData  in chapterRegionDataList

                    join leftJoin in (from regionData   in regionDataList
                                      join tileSetData  in tileSetDataList on regionData.TileSetId equals tileSetData.Id

                                      join leftJoin in (from tileData in tileDataList
                                                        select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                                      select new { regionData, tileData }) on chapterRegionData.RegionId equals leftJoin.regionData.Id into regionData
                                      
                    select new ChapterRegionElementData()
                    {
                        Id = chapterRegionData.Id,

                        ChapterId = chapterRegionData.ChapterId,
                        RegionId = chapterRegionData.RegionId,

                        Name = regionData.FirstOrDefault() != null ? regionData.FirstOrDefault().regionData.Name : "",

                        TileIconPath = regionData.FirstOrDefault() != null ? regionData.FirstOrDefault().tileData.First().tileData.IconPath : ""

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());
        
        return list.Cast<IElementData>().ToList();
    }

    public static ChapterRegionElementData DefaultData()
    {
        return new ChapterRegionElementData()
        {
            Id = -1
        };
    }

    public static void SetDefaultAddValues(List<ChapterRegionElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetChapterRegionData(Search.ChapterRegion searchParameters)
    {
        chapterRegionDataList = new List<ChapterRegionBaseData>();

        foreach(ChapterRegionBaseData chapterRegion in Fixtures.chapterRegionList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(chapterRegion.Id))                 continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(chapterRegion.ChapterId))   continue;

            var chapterRegionData = new ChapterRegionBaseData();

            chapterRegionData.Id = chapterRegion.Id;

            chapterRegionData.ChapterId = chapterRegion.ChapterId;
            chapterRegionData.RegionId = chapterRegion.RegionId;

            chapterRegionDataList.Add(chapterRegionData);
        }
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = chapterRegionDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
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

    public static void AddData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.chapterRegionList.Count > 0 ? (Fixtures.chapterRegionList[Fixtures.chapterRegionList.Count - 1].Id + 1) : 1;
            Fixtures.chapterRegionList.Add(((ChapterRegionData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

            SaveDataManager.UpdateReferences(dataRequest);

        } else { }
    }

    private static void AddDependencies(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        if (!dataRequest.includeDependencies) return;

        AddPhaseRegionData(elementData, dataRequest);
    }

    private static void AddPhaseRegionData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        //Chapter
        var chapterSearchParameters = new Search.Chapter()
        {
            id = new List<int>() { elementData.ChapterId }
        };

        var chapterData = DataManager.GetChapterData(chapterSearchParameters).FirstOrDefault();

        //Phase
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = new List<int>() { chapterData.Id }
        };

        var phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);

        if (phaseDataList.Count == 0) return;

        phaseDataList.ForEach(phaseData =>
        {
            var phaseElementData = new PhaseElementData()
            {
                Id = phaseData.Id,
                ChapterId = phaseData.ChapterId,
                DefaultRegionId = phaseData.DefaultRegionId
            };

            phaseElementData.SetOriginalValues();

            PhaseDataManager.CopyRegionData(phaseElementData, elementData, dataRequest);
        });
    }

    public static void UpdateData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.chapterRegionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedRegionId)
            {
                data.RegionId = elementData.RegionId;
                
                ReplacePhaseRegion(elementData, dataRequest);
            }

            elementData.SetOriginalValues();

        } else {

            if (elementData.ChangedRegionId)
            {
                ReplacePhaseRegion(elementData, dataRequest);
            }
        }
    }

    private static void ReplacePhaseRegion(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        RemovePhaseRegionData(elementData, dataRequest);
        AddPhaseRegionData(elementData, dataRequest);
    }

    public static void RemoveData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.chapterRegionList.RemoveAll(x => x.Id == elementData.Id);

            SaveDataManager.UpdateReferences(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        RemovePhaseRegionData(elementData, dataRequest);
    }

    private static void RemovePhaseRegionData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        //Region
        var regionSearchParameters = new Search.Region()
        {
            chapterRegionId = new List<int>() { elementData.Id }
        };

        var regionDataList = DataManager.GetRegionData(regionSearchParameters);

        if (regionDataList.Count == 0) return;

        regionDataList.ForEach(regionData =>
        {
            var regionElementData = new RegionElementData()
            {
                Id = regionData.Id
            };

            regionElementData.Remove(dataRequest);
        });

        //Phase
        var phaseSearchParameters = new Search.Phase()
        {
            id = regionDataList.Select(x => x.PhaseId).ToList()
        };

        var phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);

        phaseDataList.ForEach(phaseData =>
        {
            var phaseElementData = new PhaseElementData()
            {
                Id = phaseData.Id,
                DefaultRegionId = phaseData.DefaultRegionId,

                DefaultPositionX = phaseData.DefaultPositionX,
                DefaultPositionY = phaseData.DefaultPositionY,
                DefaultPositionZ = phaseData.DefaultPositionZ,

                DefaultRotationX = phaseData.DefaultRotationX,
                DefaultRotationY = phaseData.DefaultRotationY,
                DefaultRotationZ = phaseData.DefaultRotationZ
            };

            phaseElementData.SetOriginalValues();

            PhaseDataManager.UpdateDefaultRegion(phaseElementData, dataRequest);
        });
    }
}
