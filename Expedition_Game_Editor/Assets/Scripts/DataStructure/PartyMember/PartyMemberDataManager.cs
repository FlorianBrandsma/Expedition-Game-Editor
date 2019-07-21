﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyMemberDataManager
{
    private PartyMemberController partyMemberController;
    private List<PartyMemberData> partyMemberDataList = new List<PartyMemberData>();

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(PartyMemberController partyMemberController)
    {
        this.partyMemberController = partyMemberController;
    }

    public List<IDataElement> GetPartyMemberDataElements(IEnumerable searchParameters)
    {
        var partyMemberSearchData = searchParameters.Cast<Search.PartyMember>().FirstOrDefault();

        switch (partyMemberSearchData.requestType)
        {
            case Search.PartyMember.RequestType.Custom:

                GetCustomPartyMemberData(partyMemberSearchData);
                break;
        }

        if (partyMemberDataList.Count == 0) return new List<IDataElement>();

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from partyMemberData in partyMemberDataList
                    join interactableData in interactableDataList on partyMemberData.interactableId equals interactableData.id
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new PartyMemberDataElement()
                    {
                        dataType = Enums.DataType.PartyMember,

                        id = partyMemberData.id,
                        
                        ChapterId = partyMemberData.chapterId,
                        InteractableId = partyMemberData.interactableId,

                        interactableName = interactableData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomPartyMemberData(Search.PartyMember searchParameters)
    {
        partyMemberDataList = new List<PartyMemberData>();

        foreach (Fixtures.PartyMember partyMember in Fixtures.partyMemberList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(partyMember.id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(partyMember.chapterId)) continue;

            var partyMemberData = new PartyMemberData();

            partyMemberData.id = partyMember.id;

            partyMemberData.chapterId = partyMember.chapterId;
            partyMemberData.interactableId = partyMember.interactableId;

            partyMemberDataList.Add(partyMemberData);
        }
    }

    internal void GetInteractableData()
    {
        interactableDataList = dataManager.GetInteractableData(partyMemberDataList.Select(x => x.interactableId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class PartyMemberData : GeneralData
    {
        public int chapterId;
        public int interactableId;
    }
}