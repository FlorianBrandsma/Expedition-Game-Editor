using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterSaveDataManager
{
    private static List<ChapterSaveBaseData> chapterSaveDataList;

    private static List<ChapterBaseData> chapterDataList;

    public static List<IElementData> GetData(Search.ChapterSave searchParameters)
    {
        GetChapterSaveData(searchParameters);

        if (chapterSaveDataList.Count == 0) return new List<IElementData>();
        
        GetChapterData();

        var list = (from chapterSaveData    in chapterSaveDataList
                    join chapterData        in chapterDataList on chapterSaveData.ChapterId equals chapterData.Id
                    select new ChapterSaveElementData()
                    {
                        Id = chapterSaveData.Id,
                        
                        SaveId = chapterSaveData.SaveId,
                        ChapterId = chapterSaveData.ChapterId,

                        Complete = chapterSaveData.Complete,

                        Index = chapterData.Index,

                        Name = chapterData.Name,

                        PublicNotes = chapterData.PublicNotes,
                        PrivateNotes = chapterData.PrivateNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();
        
        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        chapterSaveDataList = new List<ChapterSaveBaseData>();

        foreach (ChapterSaveBaseData chapterSave in Fixtures.chapterSaveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(chapterSave.Id))           continue;
            if (searchParameters.saveId.Count   > 0 && !searchParameters.saveId.Contains(chapterSave.SaveId))   continue;

            chapterSaveDataList.Add(chapterSave);
        }
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = chapterSaveDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    public static void UpdateData(ChapterSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.chapterSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedComplete)
            {
                data.Complete = elementData.Complete;
            }

            elementData.SetOriginalValues();

        } else { }
    }
}
