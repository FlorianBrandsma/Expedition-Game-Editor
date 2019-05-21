using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterElementDataManager
{
    private ChapterElementController chapterElementController;
    private List<ChapterElementData> chapterElementDataList;

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.ElementData> elementDataList;

    private DataManager dataManager = new DataManager();

    public void InitializeManager(ChapterElementController chapterElementController)
    {
        this.chapterElementController = chapterElementController;
    }

    public List<ChapterElementDataElement> GetChapterElementDataElements(IEnumerable searchParameters)
    {
        var chapterElementSearchData = searchParameters.Cast<Search.ChapterElement>().FirstOrDefault();

        switch(chapterElementSearchData.requestType)
        {
            case Search.ChapterElement.RequestType.Custom:

                GetCustomChapterElementData(chapterElementSearchData);
                break;

            case Search.ChapterElement.RequestType.GetChapterElementsById:

                GetChapterElementById(chapterElementSearchData);
                break;
        }

        GetElementData();
        GetObjectGraphicData();

        var list = (from chapterElementData in chapterElementDataList
                    join elementData in elementDataList on chapterElementData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new ChapterElementDataElement()
                    {
                        id = chapterElementData.id,
                        table = chapterElementData.table,

                        ChapterId = chapterElementData.chapterId,
                        ElementId = chapterElementData.elementId,

                        objectGraphicIcon = objectGraphicData.icon

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetCustomChapterElementData(Search.ChapterElement searchParameters)
    {
        chapterElementDataList = new List<ChapterElementData>();

        foreach(Fixtures.ChapterElement chapterElement in Fixtures.chapterElementList)
        {
            var chapterElementData = new ChapterElementData();
            
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapterElement.id)) continue;

            chapterElementData.id = chapterElement.id;
            chapterElementData.table = "ChapterElement";
            
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(chapterElement.chapterId)) continue;

            chapterElementData.chapterId = chapterElement.chapterId;
            chapterElementData.elementId = chapterElement.elementId;

            chapterElementDataList.Add(chapterElementData);
        }
    }

    internal void GetChapterElementById(Search.ChapterElement searchParameters)
    {
        
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetElementData(chapterElementDataList.Select(x => x.elementId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class ChapterElementData : GeneralData
    {
        public int chapterId;
        public int elementId;
    }
}
