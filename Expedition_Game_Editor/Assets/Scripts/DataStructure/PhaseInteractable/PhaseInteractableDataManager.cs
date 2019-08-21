﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseInteractableDataManager
{
    private PhaseInteractableController phaseInteractableController;
    private List<PhaseInteractableData> phaseInteractableDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.SceneInteractableData> sceneInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(PhaseInteractableController phaseInteractableController)
    {
        this.phaseInteractableController = phaseInteractableController;
    }

    public List<IDataElement> GetQuestInteractableDataElements(IEnumerable searchParameters)
    {
        var phaseInteractableSearchData = searchParameters.Cast<Search.PhaseInteractable>().FirstOrDefault();

        GetPhaseInteractableData(phaseInteractableSearchData);

        GetSceneInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from phaseInteractableData  in phaseInteractableDataList
                    join sceneInteractableData  in sceneInteractableDataList    on phaseInteractableData.sceneInteractableId    equals sceneInteractableData.id
                    join interactableData       in interactableDataList         on sceneInteractableData.interactableId         equals interactableData.id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId             equals objectGraphicData.id
                    join iconData               in iconDataList                 on objectGraphicData.iconId                     equals iconData.id
                    select new PhaseInteractableDataElement()
                    {
                        dataType = Enums.DataType.PhaseInteractable,

                        id = phaseInteractableData.id,
                        index = phaseInteractableData.index,

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(phaseInteractable.id)) continue;
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(phaseInteractable.phaseId)) continue;

            var phaseInteractableData = new PhaseInteractableData();

            phaseInteractableData.id = phaseInteractable.id;
            phaseInteractableData.index = phaseInteractable.index;

            phaseInteractableData.phaseId = phaseInteractable.phaseId;
            phaseInteractableData.questId = phaseInteractable.questId;
            phaseInteractableData.sceneInteractableId = phaseInteractable.sceneInteractableId;

            phaseInteractableDataList.Add(phaseInteractableData);
        }
    }

    internal void GetSceneInteractableData()
    {
        sceneInteractableDataList = dataManager.GetSceneInteractableData(phaseInteractableDataList.Select(x => x.sceneInteractableId).Distinct().ToList(), true);
    }

    internal void GetInteractableData()
    {
        interactableDataList = dataManager.GetInteractableData(sceneInteractableDataList.Select(x => x.interactableId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    Enums.ElementStatus GetElementStatus(PhaseInteractableData phaseData)
    {
        var questData = (QuestDataElement)phaseInteractableController.SegmentController.editorController.PathController.route.data.dataElement;

        if (phaseData.questId == questData.id)
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
