using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseInteractableDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<PhaseInteractableData> phaseInteractableDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ChapterInteractableData> chapterInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public PhaseInteractableDataManager(PhaseInteractableController phaseInteractableController)
    {
        DataController = phaseInteractableController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseInteractable>().First();

        GetPhaseInteractableData(searchParameters);

        if (phaseInteractableDataList.Count == 0) return new List<IDataElement>();

        GetChapterInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from phaseInteractableData      in phaseInteractableDataList
                    join chapterInteractableData    in chapterInteractableDataList  on phaseInteractableData.chapterInteractableId  equals chapterInteractableData.Id
                    join interactableData           in interactableDataList         on chapterInteractableData.interactableId       equals interactableData.Id
                    join objectGraphicData          in objectGraphicDataList        on interactableData.objectGraphicId             equals objectGraphicData.Id
                    join iconData                   in iconDataList                 on objectGraphicData.iconId                     equals iconData.Id
                    select new PhaseInteractableDataElement()
                    {
                        Id = phaseInteractableData.Id,
                        Index = phaseInteractableData.Index,

                        PhaseId = phaseInteractableData.phaseId,
                        ChapterInteractableId = phaseInteractableData.chapterInteractableId,
                        QuestId = phaseInteractableData.questId,
                        
                        elementStatus = GetElementStatus(phaseInteractableData),
                        interactableName = interactableData.name,
                        objectGraphicIcon = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetPhaseInteractableData(Search.PhaseInteractable searchParameters)
    {
        phaseInteractableDataList = new List<PhaseInteractableData>();

        foreach(Fixtures.PhaseInteractable phaseInteractable in Fixtures.phaseInteractableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(phaseInteractable.Id)) continue;
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(phaseInteractable.phaseId)) continue;

            var phaseInteractableData = new PhaseInteractableData();

            phaseInteractableData.Id = phaseInteractable.Id;
            phaseInteractableData.Index = phaseInteractable.Index;

            phaseInteractableData.phaseId = phaseInteractable.phaseId;
            phaseInteractableData.questId = phaseInteractable.questId;
            phaseInteractableData.chapterInteractableId = phaseInteractable.chapterInteractableId;

            phaseInteractableDataList.Add(phaseInteractableData);
        }
    }

    internal void GetChapterInteractableData()
    {
        var chapterInteractableSearchParameters = new Search.ChapterInteractable();

        chapterInteractableSearchParameters.id = phaseInteractableDataList.Select(x => x.chapterInteractableId).Distinct().ToList();

        chapterInteractableDataList = dataManager.GetChapterInteractableData(chapterInteractableSearchParameters);
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

    Enums.ElementStatus GetElementStatus(PhaseInteractableData phaseData)
    {
        var questData = (QuestDataElement)DataController.SegmentController.editorController.PathController.route.data.dataElement;

        if (phaseData.questId == questData.Id)
            return Enums.ElementStatus.Enabled;
        else if (phaseData.questId == 0)
            return Enums.ElementStatus.Disabled;
        else
            return Enums.ElementStatus.Locked;
    }

    internal class PhaseInteractableData : GeneralData
    {
        public int phaseId;
        public int chapterInteractableId;
        public int questId;
    }
}
