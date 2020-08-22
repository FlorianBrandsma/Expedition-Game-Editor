using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ChapterSaveData> chapterSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ChapterData> chapterDataList;

    public ChapterSaveDataManager(ChapterSaveController chapterController)
    {
        DataController = chapterController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterSave>().First();

        GetChapterSaveData(searchParameters);

        if (chapterSaveDataList.Count == 0) return new List<IElementData>();

        GetChapterData();

        var list = (from chapterSaveData    in chapterSaveDataList
                    join chapterData        in chapterDataList on chapterSaveData.chapterId equals chapterData.id
                    select new ChapterSaveElementData()
                    {
                        Id = chapterSaveData.id,
                        Index = chapterSaveData.index,
                        
                        SaveId = chapterSaveData.saveId,
                        ChapterId = chapterSaveData.chapterId,

                        Complete = chapterSaveData.complete,

                        name = chapterData.name,

                        publicNotes = chapterData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        chapterSaveDataList = new List<ChapterSaveData>();

        foreach (Fixtures.ChapterSave chapterSave in Fixtures.chapterSaveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(chapterSave.id))           continue;
            if (searchParameters.saveId.Count   > 0 && !searchParameters.saveId.Contains(chapterSave.saveId))   continue;

            var chapterSaveData = new ChapterSaveData();

            chapterSaveData.id = chapterSave.id;
            chapterSaveData.index = chapterSave.index;

            chapterSaveData.saveId = chapterSave.saveId;
            chapterSaveData.chapterId = chapterSave.chapterId;

            chapterSaveData.complete = chapterSave.complete;

            chapterSaveDataList.Add(chapterSaveData);
        }
    }

    internal void GetChapterData()
    {
        var chapterSearchParameters = new Search.Chapter();
        chapterSearchParameters.id = chapterSaveDataList.Select(x => x.chapterId).Distinct().ToList();

        chapterDataList = dataManager.GetChapterData(chapterSearchParameters);
    }

    internal class ChapterSaveData
    {
        public int id;
        public int index;

        public int saveId;
        public int chapterId;

        public bool complete;
    }
}
