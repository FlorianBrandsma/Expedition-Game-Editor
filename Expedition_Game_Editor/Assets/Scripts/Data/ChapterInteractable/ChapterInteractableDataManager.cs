using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterInteractableDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ChapterInteractableData> chapterInteractableDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public ChapterInteractableDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First();
        
        GetChapterInteractableData(searchParameters);
        
        if (chapterInteractableDataList.Count == 0) return new List<IElementData>();

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from chapterInteractableData    in chapterInteractableDataList
                    join interactableData           in interactableDataList     on chapterInteractableData.interactableId   equals interactableData.id
                    join objectGraphicData          in objectGraphicDataList    on interactableData.objectGraphicId         equals objectGraphicData.id
                    join iconData                   in iconDataList             on objectGraphicData.iconId                 equals iconData.id
                    select new ChapterInteractableElementData()
                    {
                        Id = chapterInteractableData.id,

                        ChapterId = chapterInteractableData.chapterId,
                        InteractableId = chapterInteractableData.interactableId,

                        interactableName = interactableData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    internal void GetChapterInteractableData(Search.ChapterInteractable searchParameters)
    {
        chapterInteractableDataList = new List<ChapterInteractableData>();

        foreach (Fixtures.ChapterInteractable chapterInteractable in Fixtures.chapterInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(chapterInteractable.id)) continue;
            if (searchParameters.chapterId.Count        > 0 && !searchParameters.chapterId.Contains(chapterInteractable.chapterId)) continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(chapterInteractable.interactableId)) continue;

            var chapterInteractableData = new ChapterInteractableData();

            chapterInteractableData.id = chapterInteractable.id;

            chapterInteractableData.chapterId = chapterInteractable.chapterId;
            chapterInteractableData.interactableId = chapterInteractable.interactableId;

            chapterInteractableDataList.Add(chapterInteractableData);
        }
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = chapterInteractableDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal class ChapterInteractableData
    {
        public int id;

        public int chapterId;
        public int interactableId;
    }
}
