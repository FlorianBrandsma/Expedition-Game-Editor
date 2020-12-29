using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterRegionDataManager
{
    private static List<ChapterRegionBaseData> chapterRegionDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterRegion>().First();

        GetChapterRegionData(searchParameters);

        if (searchParameters.includeAddElement)
            chapterRegionDataList.Add(DefaultData(searchParameters));

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

    private static ChapterRegionBaseData DefaultData(Search.ChapterRegion searchParameters)
    {
        return new ChapterRegionBaseData();
    }

    private static void SetDefaultAddValues(List<ChapterRegionElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    //private static void AddDefaultElementData(Search.ChapterRegion searchParameters, List<ChapterRegionElementData> list)
    //{
    //    list.Insert(0, new ChapterRegionElementData()
    //    {
    //        ExecuteType = Enums.ExecuteType.Add
    //    });
    //}

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

            Debug.Log("Add chapter region");
        }
        else { }
    }

    public static void UpdateData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.chapterRegionList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedRegionId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
            {
                data.RegionId = elementData.RegionId;
                Debug.Log("Update chapter region");
            }
            else { }
        }
    }

    public static void RemoveData(ChapterRegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.chapterRegionList.RemoveAll(x => x.Id == elementData.Id);

            Debug.Log("Remove chapter region");
        }
        else { }
    }
}
