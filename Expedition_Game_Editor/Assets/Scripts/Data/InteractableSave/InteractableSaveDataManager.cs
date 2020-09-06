﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class InteractableSaveDataManager
{
    private static List<InteractableSaveBaseData> interactableSaveDataList;

    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractableSave>().First();

        GetInteractableSaveData(searchParameters);

        if (interactableSaveDataList.Count == 0) return new List<IElementData>();

        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from interactableSaveData   in interactableSaveDataList
                    join interactableData       in interactableDataList on interactableSaveData.InteractableId  equals interactableData.Id
                    join modelData              in modelDataList        on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList         on modelData.IconId                     equals iconData.Id
                    select new InteractableSaveElementData()
                    {
                        Id = interactableData.Id,
                        Index = interactableData.Index,

                        ModelId = interactableData.ModelId,

                        InteractableName = interactableData.Name,

                        Health = interactableData.Health,
                        Hunger = interactableData.Hunger,
                        Thirst = interactableData.Thirst,

                        Weight = interactableData.Weight,
                        Speed = interactableData.Speed,
                        Stamina = interactableData.Stamina,

                        ModelPath = modelData.Path,
                        ModelIconPath = iconData.Path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetInteractableSaveData(Search.InteractableSave searchParameters)
    {
        interactableSaveDataList = new List<InteractableSaveBaseData>();

        foreach (InteractableSaveBaseData interactableSave in Fixtures.interactableSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(interactableSave.SaveId)) continue;

            var interactableData = Fixtures.interactableList.Where(x => x.Id == interactableSave.InteractableId).First();

            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(interactableData.Type)) continue;

            var interactableSaveData = new InteractableSaveBaseData();

            interactableSaveData.Id = interactableSave.Id;
            interactableSaveData.Index = interactableSave.Index;

            interactableSaveData.SaveId = interactableSave.SaveId;
            interactableSaveData.InteractableId = interactableSave.InteractableId;

            interactableSaveDataList.Add(interactableSaveData);
        }
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = interactableSaveDataList.Select(x => x.InteractableId).Distinct().ToList();

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
}
