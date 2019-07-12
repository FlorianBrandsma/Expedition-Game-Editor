using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterDataManager
{
    private ChapterController chapterController;
    private List<ChapterData> chapterDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.InteractableData> interactableDataList;

    public void InitializeManager(ChapterController chapterController)
    {
        this.chapterController = chapterController;
    }

    public List<IDataElement> GetChapterDataElements(IEnumerable searchParameters)
    {
        var chapterSearchData = searchParameters.Cast<Search.Chapter>().FirstOrDefault();

        GetChapterData(chapterSearchData);

        GetInteractableData();
        GetObjectGraphicData();

        var list = (from chapterData in chapterDataList
                    join interactableData in interactableDataList on chapterData.interactableId equals interactableData.id
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.id
                    select new ChapterDataElement()
                    {
                        dataType = Enums.DataType.Chapter,

                        id = chapterData.id,
                        index = chapterData.index,
                        
                        InteractableId = chapterData.interactableId,
                        Name = chapterData.name,
                        Notes = chapterData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
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

            chapterData.interactableId = chapter.interactableId;
            chapterData.name = chapter.name;
            chapterData.notes = chapter.notes;
            
            chapterDataList.Add(chapterData);
        }
    }

    internal void GetInteractableData()
    {
        interactableDataList = dataManager.GetInteractableData(chapterDataList.Select(x => x.interactableId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class ChapterData : GeneralData
    {
        public int interactableId;
        public string name;
        public string notes;      
    }
}
