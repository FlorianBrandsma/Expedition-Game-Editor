using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyMemberDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<PartyMemberData> partyMemberDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public PartyMemberDataManager(PartyMemberController partyMemberController)
    {
        DataController = partyMemberController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PartyMember>().First();

        GetPartyMemberData(searchParameters);

        if (partyMemberDataList.Count == 0) return new List<IElementData>();

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from partyMemberData    in partyMemberDataList
                    join interactableData   in interactableDataList     on partyMemberData.interactableId   equals interactableData.id
                    join objectGraphicData  in objectGraphicDataList    on interactableData.objectGraphicId equals objectGraphicData.id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.id
                    select new PartyMemberElementData()
                    {
                        Id = partyMemberData.id,

                        ChapterId = partyMemberData.chapterId,
                        InteractableId = partyMemberData.interactableId,

                        interactableName = interactableData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    internal void GetPartyMemberData(Search.PartyMember searchParameters)
    {
        partyMemberDataList = new List<PartyMemberData>();

        foreach (Fixtures.PartyMember partyMember in Fixtures.partyMemberList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(partyMember.id)) continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(partyMember.chapterId)) continue;

            var partyMemberData = new PartyMemberData();

            partyMemberData.id = partyMember.id;

            partyMemberData.chapterId = partyMember.chapterId;
            partyMemberData.interactableId = partyMember.interactableId;

            partyMemberDataList.Add(partyMemberData);
        }
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = partyMemberDataList.Select(x => x.interactableId).Distinct().ToList();

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

    internal class PartyMemberData
    {
        public int id;

        public int chapterId;
        public int interactableId;
    }
}
