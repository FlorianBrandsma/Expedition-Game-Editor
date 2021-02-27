using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProjectDataManager
{
    private static List<ProjectBaseData> projectDataList;
    
    private static List<TeamBaseData> teamDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.Project searchParameters)
    {
        GetProjectData(searchParameters);

        if (searchParameters.includeAddElement)
            projectDataList.Add(DefaultData(searchParameters.teamId.First()));

        if (projectDataList.Count == 0) return new List<IElementData>();

        GetTeamData();
        GetIconData();

        var list = (from projectData    in projectDataList
                    join teamData       in teamDataList on projectData.TeamId equals teamData.Id
                    join iconData       in iconDataList on projectData.IconId equals iconData.Id
                    select new ProjectElementData()
                    {
                        Id = projectData.Id,

                        TeamId = projectData.TeamId,

                        Name = projectData.Name,
                        Description = projectData.Description,

                        TeamName = teamData.Name,

                        IconPath = iconData.Path
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Name).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static ProjectElementData DefaultData(int teamId)
    {
        return new ProjectElementData()
        {
            Id = -1,

            TeamId = teamId
        };
    }

    public static void SetDefaultAddValues(List<ProjectElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetProjectData(Search.Project searchParameters)
    {
        projectDataList = new List<ProjectBaseData>();

        foreach (ProjectBaseData project in Fixtures.projectList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(project.Id))           continue;
            if (searchParameters.teamId.Count   > 0 && !searchParameters.teamId.Contains(project.TeamId))   continue;

            projectDataList.Add(project);
        }
    }

    private static void GetTeamData()
    {
        var searchParameters = new Search.Team();
        searchParameters.id = projectDataList.Select(x => x.TeamId).Distinct().ToList();

        teamDataList = DataManager.GetTeamData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = projectDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(ProjectElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.projectList.Count > 0 ? (Fixtures.projectList[Fixtures.projectList.Count - 1].Id + 1) : 1;
            Fixtures.projectList.Add(((ProjectData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(ProjectElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.projectList.Where(x => x.Id == elementData.Id).FirstOrDefault();

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

    public static void RemoveData(ProjectElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.projectList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
