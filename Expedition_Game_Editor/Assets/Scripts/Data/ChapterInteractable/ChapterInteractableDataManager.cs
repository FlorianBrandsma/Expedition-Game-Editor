using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ChapterInteractableDataManager
{
    private static List<ChapterInteractableBaseData> chapterInteractableDataList;

    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var list = new List<ChapterInteractableElementData>();

        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First();
        
        GetChapterInteractableData(searchParameters);
        
        if (chapterInteractableDataList.Count > 0)
        {
            GetInteractableData();
            GetModelData();
            GetIconData();

            list = (from chapterInteractableData    in chapterInteractableDataList
                    join interactableData           in interactableDataList     on chapterInteractableData.InteractableId   equals interactableData.Id
                    join modelData                  in modelDataList            on interactableData.ModelId                 equals modelData.Id
                    join iconData                   in iconDataList             on modelData.IconId                         equals iconData.Id
                    select new ChapterInteractableElementData()
                    {
                        Id = chapterInteractableData.Id,

                        ChapterId = chapterInteractableData.ChapterId,
                        InteractableId = chapterInteractableData.InteractableId,

                        InteractableName = interactableData.Name,
                        ModelIconPath = iconData.Path

                    }).ToList();
        }

        if (searchParameters.includeAddElement)
            AddDefaultElementData(searchParameters, list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void AddDefaultElementData(Search.ChapterInteractable searchParameters, List<ChapterInteractableElementData> list)
    {
        list.Insert(0, new ChapterInteractableElementData()
        {
            ExecuteType = Enums.ExecuteType.Add
        });
    }

    private static void GetChapterInteractableData(Search.ChapterInteractable searchParameters)
    {
        chapterInteractableDataList = new List<ChapterInteractableBaseData>();

        foreach (ChapterInteractableBaseData chapterInteractable in Fixtures.chapterInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(chapterInteractable.Id))                           continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(chapterInteractable.ChapterId))             continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(chapterInteractable.InteractableId))   continue;

            chapterInteractableDataList.Add(chapterInteractable);
        }
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = chapterInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.worldInteractableList[Fixtures.worldInteractableList.Count - 1].Id + 1;
            Fixtures.chapterInteractableList.Add(((ChapterInteractableData)elementData).Clone());
        } else { }
    }

    public static void UpdateData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.chapterInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedInteractableId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.InteractableId = elementData.InteractableId;
        } else { }
    }

    public static void RemoveData(ChapterInteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.chapterInteractableList.RemoveAll(x => x.Id == elementData.Id);
        } else { }
    }
}
