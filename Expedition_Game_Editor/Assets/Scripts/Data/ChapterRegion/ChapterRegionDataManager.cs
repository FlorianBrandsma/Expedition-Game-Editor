using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ChapterRegionData> chapterRegionDataList;
    
    private DataManager dataManager = new DataManager();

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TileData> tileDataList;

    public ChapterRegionDataManager(ChapterRegionController chapterRegionController)
    {
        DataController = chapterRegionController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterRegion>().First();

        GetChapterRegionData(searchParameters);

        if (chapterRegionDataList.Count == 0) return new List<IElementData>();

        GetRegionData();
        GetTileSetData();
        GetTileData();

        var list = (from chapterRegionData  in chapterRegionDataList
                    join regionData         in regionDataList       on chapterRegionData.regionId   equals regionData.id
                    join tileSetData        in tileSetDataList      on regionData.tileSetId         equals tileSetData.id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.id equals leftJoin.tileData.tileSetId into tileData

                    select new ChapterRegionElementData()
                    {
                        Id = chapterRegionData.id,

                        ChapterId = chapterRegionData.chapterId,
                        RegionId = chapterRegionData.regionId,

                        name = regionData.name,

                        tileIconPath = tileData.First().tileData.iconPath

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetChapterRegionData(Search.ChapterRegion searchParameters)
    {
        chapterRegionDataList = new List<ChapterRegionData>();

        foreach(Fixtures.ChapterRegion chapterRegion in Fixtures.chapterRegionList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(chapterRegion.id)) continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(chapterRegion.chapterId)) continue;

            var chapterRegionData = new ChapterRegionData();

            chapterRegionData.id = chapterRegion.id;

            chapterRegionData.chapterId = chapterRegion.chapterId;
            chapterRegionData.regionId = chapterRegion.regionId;

            chapterRegionDataList.Add(chapterRegionData);
        }
    }

    internal void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = chapterRegionDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    private void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    private void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.id).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    internal class ChapterRegionData
    {
        public int id;

        public int chapterId;
        public int regionId;
    }
}
