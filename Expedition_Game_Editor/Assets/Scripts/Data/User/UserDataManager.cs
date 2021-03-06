﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UserDataManager
{
    private static List<UserBaseData> userDataList;
    
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.User searchParameters)
    {
        switch (searchParameters.requestType)
        {
            case Search.User.RequestType.Custom:

                GetUserData(searchParameters);
                break;

            case Search.User.RequestType.GetPotentialFavoriteUsers:

                GetPotentialFavoriteUserData(searchParameters);
                break;

            case Search.User.RequestType.GetPotentialTeamUsers:

                GetPotentialTeamUserData(searchParameters);
                break;
        }
        
        if (userDataList.Count == 0) return new List<IElementData>();
        
        GetIconData();

        var list = (from userData   in userDataList
                    join iconData   in iconDataList on userData.IconId equals iconData.Id
                    select new UserElementData()
                    {
                        Id = userData.Id,
                        
                        Username = userData.Username,
                        Email = userData.Email,

                        IconPath = iconData.Path

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetUserData(Search.User searchParameters)
    {
        userDataList = new List<UserBaseData>();

        foreach (UserBaseData user in Fixtures.userList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(user.Id)) continue;
            if (searchParameters.username != user.Username) continue;
            if (searchParameters.username == user.Username && searchParameters.password != user.Password) continue;

            userDataList.Add(user);
        }
    }

    private static void GetPotentialFavoriteUserData(Search.User searchParameters)
    {
        userDataList = new List<UserBaseData>();

        var favoriteUserList = Fixtures.favoriteUserList.Where(x => x.UserId == searchParameters.userId).ToList();

        foreach (UserBaseData user in Fixtures.userList)
        {
            if (user.Id == searchParameters.userId)                                 continue;
            if (favoriteUserList.Select(x => x.FavoriteUserId).Contains(user.Id))   continue;

            userDataList.Add(user);
        }
    }

    private static void GetPotentialTeamUserData(Search.User searchParameters)
    {
        userDataList = new List<UserBaseData>();

        var teamUserList = Fixtures.teamUserList.Where(x => x.TeamId == searchParameters.teamId).ToList();

        foreach (UserBaseData user in Fixtures.userList)
        {
            if (user.Id == searchParameters.userId) continue;
            if (teamUserList.Select(x => x.UserId).Contains(user.Id)) continue;

            userDataList.Add(user);
        }
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = userDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(UserElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.userList.Count > 0 ? (Fixtures.userList[Fixtures.userList.Count - 1].Id + 1) : 1;
            Fixtures.userList.Add(((UserData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(UserElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.userList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedIconId)
            {
                data.IconId = elementData.IconId;
            }

            if (elementData.ChangedUsername)
            {
                data.Username = elementData.Username;
            }

            if (elementData.ChangedEmail)
            {
                data.Email = elementData.Email;
            }

            if (elementData.ChangedPassword)
            {
                data.Password = elementData.Password;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(UserElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.userList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
