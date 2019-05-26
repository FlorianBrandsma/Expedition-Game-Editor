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
    private List<DataManager.ElementData> elementDataList;

    public void InitializeManager(ChapterController chapterController)
    {
        this.chapterController = chapterController;
    }

    public List<ChapterDataElement> GetChapterDataElements(IEnumerable searchParameters)
    {
        var chapterSearchData = searchParameters.Cast<Search.Chapter>().FirstOrDefault();

        GetChapterData(chapterSearchData);

        GetElementData();
        GetObjectGraphicData();

        var list = (from chapterData in chapterDataList
                    join elementData in elementDataList on chapterData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new ChapterDataElement()
                    {
                        id = chapterData.id,
                        table = chapterData.table,
                        Index = chapterData.index,

                        ElementId = chapterData.elementId,
                        Name = chapterData.name,
                        Notes = chapterData.notes,

                        objectGraphicIcon = objectGraphicData.icon

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

            chapterData.elementId = chapter.elementId;
            chapterData.name = chapter.name;
            chapterData.notes = chapter.notes;
            
            chapterDataList.Add(chapterData);
        }
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetElementData(chapterDataList.Select(x => x.elementId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class ChapterData : GeneralData
    {
        public int elementId;
        public string name;
        public string notes;      
    }
}
