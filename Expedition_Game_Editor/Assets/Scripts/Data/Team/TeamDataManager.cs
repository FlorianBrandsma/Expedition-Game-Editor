using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamDataManager
{
    private static List<TeamBaseData> teamDataList;
    
    private static List<IconBaseData> iconDataList;

    private static List<TeamUserBaseData> teamUserDataList;

    public static List<IElementData> GetData(Search.Team searchParameters)
    {
        switch (searchParameters.requestType)
        {
            case Search.Team.RequestType.Custom:
                
                GetTeamData(searchParameters);
                break;

            case Search.Team.RequestType.GetUserTeams:
                
                GetUserTeamData(searchParameters);
                break;
        }
        
        if (searchParameters.includeAddElement)
            teamDataList.Add(DefaultData());

        if (teamDataList.Count == 0) return new List<IElementData>();
        
        GetIconData();
        GetTeamMemberData();

        var list = (from teamData in teamDataList

                    join leftJoin in (from iconData in iconDataList
                                      select new { iconData }) on teamData.IconId equals leftJoin.iconData.Id into iconData

                    join leftJoin in (from teamUserData in teamUserDataList.Where(x => x.Status == (int)Enums.UserStatus.Joined)
                                      select new { teamUserData }) on teamData.Id equals leftJoin.teamUserData.TeamId into teamUserData

                    select new TeamElementData()
                    {
                        Id = teamData.Id,

                        Name = teamData.Name,
                        Description = teamData.Description,

                        IconPath = iconData.FirstOrDefault()?.iconData.Path,

                        MemberCount = teamUserData.Count()
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Name).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static TeamElementData DefaultData()
    {
        return new TeamElementData()
        {
            Id = -1
        };
    }

    public static void SetDefaultAddValues(List<TeamElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetTeamData(Search.Team searchParameters)
    {
        teamDataList = new List<TeamBaseData>();

        foreach (TeamBaseData team in Fixtures.teamList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(team.Id)) continue;

            teamDataList.Add(team);
        }
    }

    private static void GetUserTeamData(Search.Team searchParameters)
    {
        teamDataList = new List<TeamBaseData>();

        //Get the teams that this user belongs to
        var teamUserList = Fixtures.teamUserList.Where(x => x.UserId == searchParameters.userId).ToList();

        //Get associated users who actually joined the team or not, based on the search parameter value
        if(searchParameters.associated)
            teamUserList = teamUserList.Where(x => (x.Status == (int)Enums.UserStatus.Joined) == searchParameters.joined).ToList();
    
        foreach (TeamBaseData team in Fixtures.teamList)
        {
            if ((teamUserList.Select(x => x.TeamId).Contains(team.Id)) != searchParameters.associated) continue;

            teamDataList.Add(team);
        }
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = teamDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    private static void GetTeamMemberData()
    {
        var searchParameters = new Search.TeamUser();
        searchParameters.teamId = teamDataList.Select(x => x.Id).Distinct().ToList();

        teamUserDataList = DataManager.GetTeamUserData(searchParameters);
    }

    public static void AddData(TeamElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.teamList.Count > 0 ? (Fixtures.teamList[Fixtures.teamList.Count - 1].Id + 1) : 1;
            Fixtures.teamList.Add(((TeamData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

        } else { }
    }

    private static void AddDependencies(TeamElementData elementData, DataRequest dataRequest)
    {
        AddTeamUserData(elementData, dataRequest);
    }

    private static void AddTeamUserData(TeamElementData elementData, DataRequest dataRequest)
    {
        var teamUserElementData = TeamUserDataManager.DefaultData(elementData.Id);

        teamUserElementData.Role = (int)Enums.UserRole.ProjectManager;

        teamUserElementData.Add(dataRequest);
    }

    public static void UpdateData(TeamElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.teamList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedIconId)
            {
                data.IconId = elementData.IconId;
            }

            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedDescription)
            {
                data.Description = elementData.Description;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(TeamElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.teamList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
