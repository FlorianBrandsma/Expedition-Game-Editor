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

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterSave>().First();

        GetChapterSaveData(searchParameters);

        if (chapterSaveDataList.Count == 0) return new List<IDataElement>();

        GetChapterData();

        var list = (from chapterSaveData    in chapterSaveDataList
                    join chapterData        in chapterDataList on chapterSaveData.chapterId equals chapterData.Id
                    select new ChapterSaveDataElement()
                    {
                        Id = chapterSaveData.Id,
                        
                        GameSaveId = chapterSaveData.gameSaveId,
                        ChapterId = chapterSaveData.chapterId,

                        Complete = chapterSaveData.complete,

                        name = chapterData.name,

                        publicNotes = chapterData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetChapterSaveData(Search.ChapterSave searchParameters)
    {
        chapterSaveDataList = new List<ChapterSaveData>();

        foreach (Fixtures.ChapterSave chapterSave in Fixtures.chapterSaveList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapterSave.Id)) return;
            if (searchParameters.gameSaveId.Count > 0 && !searchParameters.gameSaveId.Contains(chapterSave.gameSaveId)) return;

            var chapterSaveData = new ChapterSaveData();

            chapterSaveData.Id = chapterSave.Id;

            chapterSaveData.gameSaveId = chapterSave.gameSaveId;
            chapterSaveData.chapterId = chapterSave.chapterId;

            chapterSaveDataList.Add(chapterSaveData);
        }
    }

    internal void GetChapterData()
    {
        var chapterSearchParameters = new Search.Chapter();
        chapterSearchParameters.id = chapterSaveDataList.Select(x => x.chapterId).Distinct().ToList();

        chapterDataList = dataManager.GetChapterData(chapterSearchParameters);
    }

    internal class ChapterSaveData : GeneralData
    {
        public int gameSaveId;
        public int chapterId;

        public bool complete;
    }
}
