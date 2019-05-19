using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionDataManager
{
    private ChapterRegionController chapterRegionController;
    private List<ChapterRegionData> chapterRegionDataList;

    public void InitializeManager(ChapterRegionController chapterRegionController)
    {
        this.chapterRegionController = chapterRegionController;
    }

    public List<ChapterRegionDataElement> GetChapterRegionDataElements(IEnumerable searchParameters)
    {
        var chapterRegionSearchData = searchParameters.Cast<Search.ChapterRegion>().FirstOrDefault();

        GetChapterRegionData(chapterRegionSearchData);

        var list = (from chapterRegionData in chapterRegionDataList
                    select new ChapterRegionDataElement()
                    {
                        id = chapterRegionData.id,
                        table = chapterRegionData.table,

                        Index = chapterRegionData.index,
                        Name = chapterRegionData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetChapterRegionData(Search.ChapterRegion searchParameters)
    {
        chapterRegionDataList = new List<ChapterRegionData>();

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var chapterRegionData = new ChapterRegionData();

            chapterRegionData.id = (i + 1);
            chapterRegionData.table = "ChapterRegion";
            chapterRegionData.index = i;

            chapterRegionData.name = "ChapterRegion " + (i + 1);

            chapterRegionDataList.Add(chapterRegionData);
        }
    }

    internal class ChapterRegionData : GeneralData
    {
        public int index;
        public string name;
    }
}
