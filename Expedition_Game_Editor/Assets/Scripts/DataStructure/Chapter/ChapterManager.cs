using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterManager
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
                        type = chapterData.type,
                        Index = chapterData.index,
                        Name = chapterData.name,
                        Description = chapterData.description,

                        elementIds = new List<int>() { 1, 2 }

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetChapterData(Search.Chapter searchParameters)
    {
        chapterDataList = new List<ChapterData>();

        //Temporary
        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var chapterData = new ChapterData();

            int id = (i + 1);

            if (searchParameters.exclusedIdList.Contains(id)) continue;
            
            chapterData.id = id;
            chapterData.table = "Chapter";
            chapterData.index = i;

            chapterData.name = "Chapter " + id;
            chapterData.description = "This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            chapterDataList.Add(chapterData);
        }
    }

    internal class ChapterData : GeneralData
    {
        public int index;
        public string name;
        public string description;      
    }
}
