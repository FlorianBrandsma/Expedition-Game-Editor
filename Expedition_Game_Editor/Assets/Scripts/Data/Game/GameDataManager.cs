using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameDataManager
{
    private static List<GameBaseData> gameDataList;

    private static List<ProjectBaseData> projectDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.Game searchParameters)
    {
        GetGameData(searchParameters);

        if (searchParameters.includeAddElement)
            gameDataList.Add(DefaultData());

        if (gameDataList.Count == 0) return new List<IElementData>();

        GetProjectData();
        GetIconData();

        var list = (from gameData       in gameDataList
                    join projectData    in projectDataList  on gameData.ProjectId equals projectData.Id
                    join iconData       in iconDataList     on projectData.IconId equals iconData.Id
                    select new GameElementData()
                    {
                        Id = gameData.Id,

                        ProjectId = gameData.ProjectId,

                        Rating = gameData.Rating,

                        IconPath = iconData.Path,

                        Name = projectData.Name,
                        Description = projectData.Description
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Name).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static GameElementData DefaultData()
    {
        return new GameElementData()
        {
            Id = -1
        };
    }

    public static void SetDefaultAddValues(List<GameElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetGameData(Search.Game searchParameters)
    {
        gameDataList = new List<GameBaseData>();

        foreach (GameBaseData game in Fixtures.gameList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(game.Id)) continue;

            gameDataList.Add(game);
        }
    }

    private static void GetProjectData()
    {
        var searchParameters = new Search.Project();
        searchParameters.id = gameDataList.Select(x => x.ProjectId).Distinct().ToList();

        projectDataList = DataManager.GetProjectData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = projectDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(GameElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.gameList.Count > 0 ? (Fixtures.gameList[Fixtures.gameList.Count - 1].Id + 1) : 1;
            Fixtures.gameList.Add(((GameData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(GameElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.gameList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedRating)
            {
                data.Rating = elementData.Rating;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(GameElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.gameList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
