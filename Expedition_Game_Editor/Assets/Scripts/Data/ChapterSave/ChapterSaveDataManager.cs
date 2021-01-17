using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterSaveDataManager
{
    private static List<ChapterBaseData> chapterDataList;

    private static List<ChapterSaveBaseData> chapterSaveDataList;
    
    public static List<IElementData> GetData(Search.ChapterSave searchParameters)
    {
        GetChapterData();
        
        if (chapterDataList.Count == 0) return new List<IElementData>();

        GetChapterSaveData(searchParameters);

        var list = (from chapterData        in chapterDataList
                    join chapterSaveData    in chapterSaveDataList on chapterData.Id equals chapterSaveData.ChapterId
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

    public static ChapterSaveElementData DefaultData(int saveId, int chapterId)
    {
        return new ChapterSaveElementData()
        {
            Id = -1,

            SaveId = saveId,
            ChapterId = chapterId
        };
    }

    private static void GetChapterData()
    {
        chapterDataList = new List<ChapterBaseData>();

        foreach (ChapterBaseData chapter in Fixtures.chapterList)
        {
            chapterDataList.Add(chapter);
        }
    }

    private static void GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        searchParameters.chapterId = chapterDataList.Select(x => x.Id).Distinct().ToList();

        chapterSaveDataList = DataManager.GetChapterSaveData(searchParameters);
    }
    
    public static void AddData(ChapterSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.chapterSaveList.Count > 0 ? (Fixtures.chapterSaveList[Fixtures.chapterSaveList.Count - 1].Id + 1) : 1;
            Fixtures.chapterSaveList.Add(((ChapterSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
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

                PlayerSaveDataManager.UpdateReferences(dataRequest);
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(ChapterSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.chapterSaveList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
