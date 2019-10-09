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
    private List<DataManager.InteractableData> interactableDataList;

    public ChapterDataManager(ChapterController chapterController)
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
                    join interactableData in interactableDataList on chapterData.interactableId equals interactableData.Id
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.Id
                    select new ChapterDataElement()
                    {
                        dataType = Enums.DataType.Chapter,

                        Id = chapterData.Id,
                        Index = chapterData.Index,
                        
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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(chapter.Id)) return;

            var chapterData = new ChapterData();
            
            chapterData.Id = chapter.Id;
            chapterData.Index = chapter.Index;

            chapterData.interactableId = chapter.interactableId;
            chapterData.name = chapter.name;
            chapterData.notes = chapter.notes;
            
            chapterDataList.Add(chapterData);
        }
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = chapterDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal class ChapterData : GeneralData
    {
        public int interactableId;
        public string name;
        public string notes;      
    }
}
