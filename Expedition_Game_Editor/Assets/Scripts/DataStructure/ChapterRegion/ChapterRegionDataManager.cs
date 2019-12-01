using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<ChapterRegionData> chapterRegionDataList;
    private List<DataManager.RegionData> regionDataList;

    private DataManager dataManager = new DataManager();

    public ChapterRegionDataManager(ChapterRegionController chapterRegionController)
    {
        DataController = chapterRegionController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var chapterRegionSearchData = searchParameters.Cast<Search.ChapterRegion>().FirstOrDefault();

        GetChapterRegionData(chapterRegionSearchData);

        GetRegionData();

        var list = (from chapterRegionData in chapterRegionDataList
                    join regionData in regionDataList on chapterRegionData.regionId equals regionData.Id
                    select new ChapterRegionDataElement()
                    {
                        DataType = Enums.DataType.ChapterRegion,

                        Id = chapterRegionData.Id,
                        Index = chapterRegionData.Index,

                        ChapterId = chapterRegionData.chapterId,
                        RegionId = chapterRegionData.regionId,

                        name = regionData.name

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

    internal class ChapterRegionData : GeneralData
    {
        public int chapterId;
        public int regionId;
    }
}
