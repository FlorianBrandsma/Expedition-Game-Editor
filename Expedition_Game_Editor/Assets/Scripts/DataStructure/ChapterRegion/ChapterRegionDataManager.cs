using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionDataManager
{
    private ChapterRegionController chapterRegionController;
    private List<ChapterRegionData> chapterRegionDataList;
    private List<DataManager.RegionData> regionDataList;

    private DataManager dataManager = new DataManager();

    public void InitializeManager(ChapterRegionController chapterRegionController)
    {
        this.chapterRegionController = chapterRegionController;
    }

    public List<IDataElement> GetChapterRegionDataElements(IEnumerable searchParameters)
    {
        var chapterRegionSearchData = searchParameters.Cast<Search.ChapterRegion>().FirstOrDefault();

        GetChapterRegionData(chapterRegionSearchData);

        GetRegionData();

        var list = (from chapterRegionData in chapterRegionDataList
                    join regionData in regionDataList on chapterRegionData.regionId equals regionData.id
                    select new ChapterRegionDataElement()
                    {
                        dataType = Enums.DataType.ChapterRegion,

                        id = chapterRegionData.id,
                        Index = chapterRegionData.index,

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapterRegion.id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(chapterRegion.chapterId)) continue;

            var chapterRegionData = new ChapterRegionData();

            chapterRegionData.id = chapterRegion.id;
            chapterRegionData.index = chapterRegion.id;
            
            chapterRegionData.chapterId = chapterRegion.chapterId;
            chapterRegionData.regionId = chapterRegion.regionId;

            chapterRegionDataList.Add(chapterRegionData);
        }
    }

    internal void GetRegionData()
    {
        regionDataList = dataManager.GetRegionData(chapterRegionDataList.Select(x => x.regionId).Distinct().ToList(), true);
    }

    internal class ChapterRegionData : GeneralData
    {
        public int chapterId;
        public int regionId;
    }
}
