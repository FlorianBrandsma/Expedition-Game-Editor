using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterDataManager
{
    private ChapterController chapterController;
    private List<ChapterData> chapterDataList;

    public void InitializeManager(ChapterController chapterController)
    {
        this.chapterController = chapterController;
    }

    public List<ChapterDataElement> GetChapterDataElements(IEnumerable searchParameters)
    {
        var chapterSearchData = searchParameters.Cast<Search.Chapter>().FirstOrDefault();

        GetChapterData(chapterSearchData);

        var list = (from chapterData in chapterDataList
                    select new ChapterDataElement()
                    {
                        id = chapterData.id,
                        table = chapterData.table,

                        Index = chapterData.index,
                        Name = chapterData.name,
                        Notes = chapterData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetChapterData(Search.Chapter searchParameters)
    {
        chapterDataList = new List<ChapterData>();

        foreach(Fixtures.Chapter chapter in Fixtures.chapterList)
        {
            var chapterData = new ChapterData();
            
            chapterData.id = chapter.id;
            chapterData.table = "Chapter";
            chapterData.index = chapter.index;

            chapterData.name = chapter.name;
            chapterData.notes = chapter.notes;

            chapterDataList.Add(chapterData);
        }
    }

    internal class ChapterData : GeneralData
    {
        public string name;
        public string notes;      
    }
}
