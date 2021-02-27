using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamUserDataManager
{
    private static List<TeamUserBaseData> teamUserDataList;

    private static List<UserBaseData> userDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.TeamUser searchParameters)
    {
        GetTeamUserData(searchParameters);

        if (searchParameters.includeAddElement)
            teamUserDataList.Add(DefaultData(searchParameters.teamId.First()));

        if (teamUserDataList.Count == 0) return new List<IElementData>();

        GetUserData();
        GetIconData();

        var list = (from teamUserData   in teamUserDataList
                    join userData       in userDataList on teamUserData.UserId  equals userData.Id
                    join iconData       in iconDataList on userData.IconId      equals iconData.Id
                    select new TeamUserElementData()
                    {
                        Id = teamUserData.Id,

                        Role = teamUserData.Role,

                        IconPath = iconData.Path,
                        Username = userData.Username

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Role).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static TeamUserElementData DefaultData(int teamId)
    {
        return new TeamUserElementData()
        {
            Id = -1,

            TeamId = teamId
        };
    }

    public static void SetDefaultAddValues(List<TeamUserElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetTeamUserData(Search.TeamUser searchParameters)
    {
        teamUserDataList = new List<TeamUserBaseData>();

        foreach (TeamUserBaseData teamUser in Fixtures.teamUserList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(teamUser.Id))          continue;
            if (searchParameters.teamId.Count   > 0 && !searchParameters.teamId.Contains(teamUser.TeamId))  continue;
            if (searchParameters.status     != null && searchParameters.status != teamUser.Status)          continue;

            teamUserDataList.Add(teamUser);
        }
    }

    private static void GetUserData()
    {
        var searchParameters = new Search.User();
        searchParameters.id = teamUserDataList.Select(x => x.UserId).Distinct().ToList();

        userDataList = DataManager.GetUserData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = userDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(TeamUserElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.teamUserList.Count > 0 ? (Fixtures.teamUserList[Fixtures.teamUserList.Count - 1].Id + 1) : 1;
            Fixtures.teamUserList.Add(((TeamUserData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(TeamUserElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.teamUserList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedRole)
            {
                data.Role = elementData.Role;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(TeamUserElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.teamUserList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
