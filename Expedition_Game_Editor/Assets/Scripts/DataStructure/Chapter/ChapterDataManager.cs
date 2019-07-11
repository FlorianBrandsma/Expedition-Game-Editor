﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterDataManager
{
    private ChapterController chapterController;
    private List<ChapterData> chapterDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.InteractableData> elementDataList;

    public void InitializeManager(ChapterController chapterController)
    {
        this.chapterController = chapterController;
    }

    public List<IDataElement> GetChapterDataElements(IEnumerable searchParameters)
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
                        dataType = Enums.DataType.Chapter,

                        id = chapterData.id,
                        index = chapterData.index,
                        
                        ElementId = chapterData.elementId,
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

            chapterData.elementId = chapter.interactableId;
            chapterData.name = chapter.name;
            chapterData.notes = chapter.notes;
            
            chapterDataList.Add(chapterData);
        }
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetInteractableData(chapterDataList.Select(x => x.elementId).Distinct().ToList(), true);
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
