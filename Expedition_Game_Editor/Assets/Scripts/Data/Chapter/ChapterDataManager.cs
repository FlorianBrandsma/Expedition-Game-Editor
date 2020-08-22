using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ChapterData> chapterDataList;

    public ChapterDataManager(ChapterController chapterController)
    {
        DataController = chapterController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Chapter>().First();

        GetChapterData(searchParameters);

        if (chapterDataList.Count == 0) return new List<IElementData>();

        var list = (from chapterData in chapterDataList
                    select new ChapterElementData()
                    {
                        Id = chapterData.id,
                        Index = chapterData.index,

                        Name = chapterData.name,

                        TimeSpeed = chapterData.timeSpeed,

                        PublicNotes = chapterData.publicNotes,
                        PrivateNotes = chapterData.privateNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());
        
        return list.Cast<IElementData>().ToList();
    }

    public void GetChapterData(Search.Chapter searchParameters)
    {
        chapterDataList = new List<ChapterData>();

        foreach(Fixtures.Chapter chapter in Fixtures.chapterList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.id)) return;

            var chapterData = new ChapterData();
            
            chapterData.id = chapter.id;
            chapterData.index = chapter.index;

            chapterData.name = chapter.name;

            chapterData.timeSpeed = chapter.timeSpeed;

            chapterData.publicNotes = chapter.publicNotes;
            chapterData.privateNotes = chapter.privateNotes;
            
            chapterDataList.Add(chapterData);
        }
    }

    internal class ChapterData
    {
        public int id;
        public int index;

        public string name;

        public float timeSpeed;

        public string publicNotes;
        public string privateNotes;
    }
}
