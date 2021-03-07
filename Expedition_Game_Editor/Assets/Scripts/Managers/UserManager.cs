using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;

    public UserElementData UserElementData          { get; set; }
    public UserDataController UserDataController    { get { return GetComponent<UserDataController>(); } }

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator Login(Action callback, string username, string password)
    {
        var searchProperties = new SearchProperties(Enums.DataType.User);

        InitializeSearchParameters(searchProperties, username, password);

        UserDataController.GetData(searchProperties);

        yield return new WaitWhile(() => UserDataController.Data == null);

        UserElementData = (UserElementData)UserDataController.Data.dataList.FirstOrDefault();

        callback();
    }

    private void InitializeSearchParameters(SearchProperties searchProperties, string username, string password)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.User>().First();

        searchParameters.username = username;
        searchParameters.password = password;
    }
}
