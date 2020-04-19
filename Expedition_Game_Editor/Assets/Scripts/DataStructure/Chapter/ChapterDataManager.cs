using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<ChapterData> chapterDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.InteractableData> interactableDataList;

    public ChapterDataManager(ChapterController chapterController)
    {
        DataController = chapterController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Chapter>().First();

        GetChapterData(searchParameters);

        if (chapterDataList.Count == 0) return new List<IDataElement>();

        GetInteractableData();
        GetObjectGraphicData();

        var list = (from chapterData        in chapterDataList
                    join interactableData   in interactableDataList     on chapterData.interactableId       equals interactableData.Id
                    join objectGraphicData  in objectGraphicDataList    on interactableData.objectGraphicId equals objectGraphicData.Id
                    select new ChapterDataElement()
                    {
                        Id = chapterData.Id,
                        Index = chapterData.Index,
                        
                        InteractableId = chapterData.interactableId,
                        Name = chapterData.name,

                        PublicNotes = chapterData.publicNotes,
                        PrivateNotes = chapterData.privateNotes

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

            chapterData.publicNotes = chapter.publicNotes;
            chapterData.privateNotes = chapter.privateNotes;
            
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

        public string publicNotes;
        public string privateNotes;
    }
}
