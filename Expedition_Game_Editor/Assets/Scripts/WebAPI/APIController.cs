using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using SimpleJSON;

public class APIController : MonoBehaviour
{
    public ExButton testAPIButton;
    public ExButton testDBButton;

    private readonly string baseURL = "https://expedition-db.herokuapp.com";

    private void OnEnable()
    {
        SetUserName("...");
        SetDBTestLabel();
    }

    public void TestAPI()
    {
        var userId = 1;

        StartCoroutine(GetUserById(userId));
    }

    public void TestDB()
    {
        DatabaseManager.AddItem();

        var items = DatabaseManager.GetItems().ToList();

        SetDBTestLabel();
    }

    private void SetUserName(string userName)
    {
        testAPIButton.label.text = "Hello " + userName + "!";
    }

    private void SetDBTestLabel()
    {
        var items = DatabaseManager.GetItems().ToList();
        testDBButton.label.text = "Length: " + items.Count + ", last added: " + items.Last().Name;
    }

    IEnumerator GetUserById(int id)
    {
        string url = baseURL + "/users/" + id;

        var webRequest = UnityWebRequest.Get(url);
        
        yield return webRequest.SendWebRequest();

        if(webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError(webRequest.error);
            yield break;
        }

        var response = JSON.Parse(webRequest.downloadHandler.text);

        var userName = response[0]["name"].Value;

        SetUserName(userName);
    }
}
