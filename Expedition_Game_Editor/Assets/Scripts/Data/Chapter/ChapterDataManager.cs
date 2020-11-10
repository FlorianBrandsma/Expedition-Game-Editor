using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterDataManager
{
    private static List<ChapterBaseData> chapterDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Chapter>().First();

        GetChapterData(searchParameters);

        if (chapterDataList.Count == 0) return new List<IElementData>();

        var list = (from chapterData in chapterDataList
                    select new ChapterElementData()
                    {
                        Id = chapterData.Id,
                        Index = chapterData.Index,

                        Name = chapterData.Name,

                        TimeSpeed = chapterData.TimeSpeed,

                        PublicNotes = chapterData.PublicNotes,
                        PrivateNotes = chapterData.PrivateNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());
        
        return list.Cast<IElementData>().ToList();
    }

    private static void GetChapterData(Search.Chapter searchParameters)
    {
        chapterDataList = new List<ChapterBaseData>();

        foreach(ChapterBaseData chapter in Fixtures.chapterList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.Id)) continue;

            chapterDataList.Add(chapter);
        }
    }

    public static void UpdateData(ChapterElementData elementData)
    {
        var data = Fixtures.chapterList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedTimeSpeed)
            data.TimeSpeed = elementData.TimeSpeed;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }

    static public void UpdateIndex(ChapterElementData elementData)
    {
        var data = Fixtures.chapterList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
