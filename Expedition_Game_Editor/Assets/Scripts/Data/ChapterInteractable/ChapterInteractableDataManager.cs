using UnityEngine;
using System.Collections;
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
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First();
        
        GetChapterInteractableData(searchParameters);
        
        if (chapterInteractableDataList.Count == 0) return new List<IElementData>();

        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from chapterInteractableData    in chapterInteractableDataList
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

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetChapterInteractableData(Search.ChapterInteractable searchParameters)
    {
        chapterInteractableDataList = new List<ChapterInteractableBaseData>();

        foreach (ChapterInteractableBaseData chapterInteractable in Fixtures.chapterInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(chapterInteractable.Id)) continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(chapterInteractable.ChapterId)) continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(chapterInteractable.InteractableId)) continue;

            var chapterInteractableData = new ChapterInteractableBaseData();

            chapterInteractableData.Id = chapterInteractable.Id;

            chapterInteractableData.ChapterId = chapterInteractable.ChapterId;
            chapterInteractableData.InteractableId = chapterInteractable.InteractableId;

            chapterInteractableDataList.Add(chapterInteractableData);
        }
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = chapterInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(ChapterInteractableElementData elementData)
    {
        var data = Fixtures.chapterInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedInteractableId)
            data.InteractableId = elementData.InteractableId;
    }
}
