using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseInteractableDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<PhaseInteractableData> phaseInteractableDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.SceneInteractableData> sceneInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public PhaseInteractableDataManager(PhaseInteractableController phaseInteractableController)
    {
        DataController = phaseInteractableController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var phaseInteractableSearchData = searchParameters.Cast<Search.PhaseInteractable>().FirstOrDefault();

        GetPhaseInteractableData(phaseInteractableSearchData);

        GetSceneInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from phaseInteractableData  in phaseInteractableDataList
                    join sceneInteractableData  in sceneInteractableDataList    on phaseInteractableData.sceneInteractableId    equals sceneInteractableData.Id
                    join interactableData       in interactableDataList         on sceneInteractableData.interactableId         equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId             equals objectGraphicData.Id
                    join iconData               in iconDataList                 on objectGraphicData.iconId                     equals iconData.Id
                    select new PhaseInteractableDataElement()
                    {
                        DataType = Enums.DataType.PhaseInteractable,

                        Id = phaseInteractableData.Id,
                        Index = phaseInteractableData.Index,

                        PhaseId = phaseInteractableData.phaseId,
                        QuestId = phaseInteractableData.questId,
                        SceneInteractableId = phaseInteractableData.sceneInteractableId,

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
            phaseInteractableData.sceneInteractableId = phaseInteractable.sceneInteractableId;

            phaseInteractableDataList.Add(phaseInteractableData);
        }
    }

    internal void GetSceneInteractableData()
    {
        var sceneInteractableSearchParameters = new Search.SceneInteractable();

        sceneInteractableSearchParameters.id = phaseInteractableDataList.Select(x => x.sceneInteractableId).Distinct().ToList();

        sceneInteractableDataList = dataManager.GetSceneInteractableData(sceneInteractableSearchParameters);
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = sceneInteractableDataList.Select(x => x.interactableId).Distinct().ToList();

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
        public int questId;
        public int phaseId;
        public int sceneInteractableId;
    }
}
