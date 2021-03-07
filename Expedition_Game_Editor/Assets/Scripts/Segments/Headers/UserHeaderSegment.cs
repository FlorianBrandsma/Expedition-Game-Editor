using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UserHeaderSegment : MonoBehaviour, ISegment
{
    public EditorElement userButton;

    public RawImage userIcon;
    public Text usernameText;

    private UserDataController UserDataController   { get { return GetComponent<UserDataController>(); } }

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    #region Properties   
    private bool LoggedIn
    {
        get { return UserManager.instance.UserElementData != null; }
    }

    private string IconPath
    {
        get { return UserManager.instance.UserElementData?.IconPath; }
    }

    private string Username
    {
        get { return UserManager.instance.UserElementData?.Username; }
    }

    public int UserId
    {
        get { return 0; }
    }
    #endregion

    public void InitializeDependencies() { }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeUserData();
    }

    private void InitializeUserData()
    {
        if(UserId > 0)
        {
            //Get user data
            var searchProperties = new SearchProperties(Enums.DataType.User);
            var searchParameters = searchProperties.searchParameters.Cast<Search.User>().First();

            searchParameters.id = new List<int>() { UserId };

            UserDataController.GetData(searchProperties);
            
        } else if (UserManager.instance.UserDataController.Data != null) {

            UserDataController.Data = UserManager.instance.UserDataController.Data;  
        }

        if (UserDataController.Data != null)
            SetUserButton();
    }

    private void SetUserButton()
    {
        var userElementData = (UserElementData)UserDataController.Data.dataList.FirstOrDefault();

        if (userElementData != null)
        {
            userButton.DataElement.Id = userElementData.Id;
            userButton.DataElement.Data = UserDataController.Data;
            userButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

            userButton.DataElement.SetElement();
        }
    }

    public void SelectUserButton()
    {
        if (userButton.DataElement.Data != null)
            userButton.SelectElement();
        else
            StartCoroutine(UserManager.instance.Login(InitializeUserData, "Florian", "test"));
    }

    public void OpenSegment()
    {
        gameObject.SetActive(true);

        StartCoroutine(UserManager.instance.Login(InitializeUserData, "Florian", "test")); 
    }
    
    public void OpenLoginForm()
    {
        //Apply user element data to the button
        //If user is null, show "login" centered
        //Else show user info
        //Other users will use the same header and they should never be null
        //Profile will be opened when selecting the button if the user isn't null
        //A different path will be opened depending on whether the opened profile
        //is that of the logged in user
        //Only if the profile is of the logged in user will the preview tab be shown

        //Render...
    }

    public void OpenProfile()
    {
        //Render...
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
