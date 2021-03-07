using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FavoriteUserDataManager
{
    private static List<FavoriteUserBaseData> favoriteUserDataList;

    private static List<UserBaseData> userDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.FavoriteUser searchParameters)
    {
        GetFavoriteUserData(searchParameters);

        if (searchParameters.includeAddElement)
            favoriteUserDataList.Add(DefaultData(searchParameters.userId));

        if (favoriteUserDataList.Count == 0) return new List<IElementData>();

        GetUserData(searchParameters);
        GetIconData();

        var list = (from favoriteUserData   in favoriteUserDataList
                    join userData           in userDataList on favoriteUserData.FavoriteUserId  equals userData.Id
                    join iconData           in iconDataList on userData.IconId                  equals iconData.Id
                    select new FavoriteUserElementData()
                    {
                        Id = favoriteUserData.Id,

                        UserId = favoriteUserData.UserId,
                        FavoriteUserId = favoriteUserData.FavoriteUserId,

                        Note = favoriteUserData.Note,

                        IconPath = iconData.Path,

                        Username = userData.Username
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Username).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static FavoriteUserElementData DefaultData(int userId)
    {
        return new FavoriteUserElementData()
        {
            Id = -1,

            UserId = userId
        };
    }

    public static void SetDefaultAddValues(List<FavoriteUserElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetFavoriteUserData(Search.FavoriteUser searchParameters)
    {
        favoriteUserDataList = new List<FavoriteUserBaseData>();

        foreach (FavoriteUserBaseData favoriteUser in Fixtures.favoriteUserList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(favoriteUser.Id)) continue;

            favoriteUserDataList.Add(favoriteUser);
        }
    }

    private static void GetUserData(Search.FavoriteUser searchData)
    {
        var userSearchParameters = new Search.User();

        userSearchParameters.id = favoriteUserDataList.Select(x => x.FavoriteUserId).Distinct().ToList();

        userDataList = DataManager.GetUserData(userSearchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = userDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(FavoriteUserElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.favoriteUserList.Count > 0 ? (Fixtures.favoriteUserList[Fixtures.favoriteUserList.Count - 1].Id + 1) : 1;
            Fixtures.favoriteUserList.Add(((FavoriteUserData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(FavoriteUserElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.favoriteUserList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedNote)
            {
                data.Note = elementData.Note;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(FavoriteUserElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.favoriteUserList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
