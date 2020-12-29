﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterDataManager
{
    private static List<ChapterBaseData> chapterDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Chapter>().First();

        GetChapterData(searchParameters);

        if (searchParameters.includeAddElement)
            chapterDataList.Add(DefaultData(searchParameters));

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

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());
        
        return list.Cast<IElementData>().ToList();
    }

    private static ChapterBaseData DefaultData(Search.Chapter searchParameters)
    {
        return new ChapterBaseData();
    }

    private static void SetDefaultAddValues(List<ChapterElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
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

    public static void AddData(ChapterElementData elementData, DataRequest dataRequest)
    {
        if(dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.chapterList.Count > 0 ? (Fixtures.chapterList[Fixtures.chapterList.Count - 1].Id + 1) : 1;
            Fixtures.chapterList.Add(((ChapterData)elementData).Clone());

        } else {

            CheckDuplicateName(elementData, dataRequest);
        }
    }

    public static void UpdateData(ChapterElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.chapterList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
            {
                data.Name = elementData.Name;

            } else {

                //Let's imagine the chapter name is unique...
                CheckDuplicateName(elementData, dataRequest);
            }
        }

        if (elementData.ChangedTimeSpeed)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
            {
                data.TimeSpeed = elementData.TimeSpeed;

            } else { }
        }

        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
            {
                data.PublicNotes = elementData.PublicNotes;

            } else { }  
        }

        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
            {
                data.PrivateNotes = elementData.PrivateNotes;

            } else { }  
        }
    }

    public static void RemoveData(ChapterElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.chapterList.RemoveAll(x => x.Id == elementData.Id);
        }
        else { }
    }

    private static void CheckDuplicateName(ChapterElementData elementData, DataRequest dataRequest)
    {
        var chapterList = Fixtures.chapterList.Where(x => x.Id != elementData.Id).ToList();

        if (chapterList.Any(x => x.Name == elementData.Name))
            dataRequest.errorList.Add("This name totally exists already");
    }

    static public void UpdateIndex(ChapterElementData elementData)
    {
        var data = Fixtures.chapterList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
