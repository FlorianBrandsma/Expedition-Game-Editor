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

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var chapterRegionSearchData = searchParameters.Cast<Search.ChapterRegion>().FirstOrDefault();

        GetChapterRegionData(chapterRegionSearchData);
        GetRegionData();
        GetTileSetData();
        GetTileData();

        var list = (from chapterRegionData  in chapterRegionDataList
                    join regionData         in regionDataList       on chapterRegionData.regionId   equals regionData.Id
                    join tileSetData        in tileSetDataList      on regionData.tileSetId         equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.tileSetId into tileData

                    select new ChapterRegionDataElement()
                    {
                        Id = chapterRegionData.Id,
                        Index = chapterRegionData.Index,

                        ChapterId = chapterRegionData.chapterId,
                        RegionId = chapterRegionData.regionId,

                        name = regionData.name,

                        tileIconPath = tileData.First().tileData.iconPath

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetChapterRegionData(Search.ChapterRegion searchParameters)
    {
        chapterRegionDataList = new List<ChapterRegionData>();

        foreach(Fixtures.ChapterRegion chapterRegion in Fixtures.chapterRegionList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapterRegion.Id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(chapterRegion.chapterId)) continue;

            var chapterRegionData = new ChapterRegionData();

            chapterRegionData.Id = chapterRegion.Id;
            chapterRegionData.Index = chapterRegion.Id;
            
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
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    internal class ChapterRegionData : GeneralData
    {
        public int chapterId;
        public int regionId;
    }
}
