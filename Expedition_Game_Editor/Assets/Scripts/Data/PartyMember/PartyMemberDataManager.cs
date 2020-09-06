using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class PartyMemberDataManager
{
    private static List<PartyMemberBaseData> partyMemberDataList;

    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PartyMember>().First();

        GetPartyMemberData(searchParameters);

        if (partyMemberDataList.Count == 0) return new List<IElementData>();

        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from partyMemberData    in partyMemberDataList
                    join interactableData   in interactableDataList on partyMemberData.InteractableId   equals interactableData.Id
                    join modelData          in modelDataList        on interactableData.ModelId         equals modelData.Id
                    join iconData           in iconDataList         on modelData.IconId                 equals iconData.Id
                    select new PartyMemberElementData()
                    {
                        Id = partyMemberData.Id,

                        ChapterId = partyMemberData.ChapterId,
                        InteractableId = partyMemberData.InteractableId,

                        InteractableName = interactableData.Name,
                        ModelIconPath = iconData.Path

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetPartyMemberData(Search.PartyMember searchParameters)
    {
        partyMemberDataList = new List<PartyMemberBaseData>();

        foreach (PartyMemberBaseData partyMember in Fixtures.partyMemberList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(partyMember.Id)) continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(partyMember.ChapterId)) continue;

            var partyMemberData = new PartyMemberBaseData();

            partyMemberData.Id = partyMember.Id;

            partyMemberData.ChapterId = partyMember.ChapterId;
            partyMemberData.InteractableId = partyMember.InteractableId;

            partyMemberDataList.Add(partyMemberData);
        }
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = partyMemberDataList.Select(x => x.InteractableId).Distinct().ToList();

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

    public static void UpdateData(PartyMemberElementData elementData)
    {
        var data = Fixtures.partyMemberList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedChapterId)
            data.ChapterId = elementData.ChapterId;

        if (elementData.ChangedInteractableId)
            data.InteractableId = elementData.InteractableId;
    }
}
