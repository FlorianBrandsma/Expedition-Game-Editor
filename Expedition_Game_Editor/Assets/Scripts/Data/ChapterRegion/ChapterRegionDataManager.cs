using UnityEngine;
using System.Collections;
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

        if (chapterRegionDataList.Count == 0) return new List<IElementData>();

        GetRegionData();
        GetTileSetData();
        GetTileData();

        var list = (from chapterRegionData  in chapterRegionDataList
                    join regionData         in regionDataList       on chapterRegionData.RegionId   equals regionData.Id
                    join tileSetData        in tileSetDataList      on regionData.TileSetId         equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                    select new ChapterRegionElementData()
                    {
                        Id = chapterRegionData.Id,

                        ChapterId = chapterRegionData.ChapterId,
                        RegionId = chapterRegionData.RegionId,

                        Name = regionData.Name,

                        TileIconPath = tileData.First().tileData.IconPath

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetChapterRegionData(Search.ChapterRegion searchParameters)
    {
        chapterRegionDataList = new List<ChapterRegionBaseData>();

        foreach(ChapterRegionBaseData chapterRegion in Fixtures.chapterRegionList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(chapterRegion.Id)) continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(chapterRegion.ChapterId)) continue;

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
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    private static void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = DataManager.GetTileData(tileSearchParameters);
    }

    public static void UpdateData(ChapterRegionElementData elementData)
    {
        var data = Fixtures.chapterRegionList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedRegionId)
            data.RegionId = elementData.RegionId;
    }
}
