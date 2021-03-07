using UnityEngine;
using UnityEngine.UI;

public class ProfileHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage userIcon;
    public Text usernameText;

    private UserElementData UserElementData         { get { return (UserElementData)SegmentController.Path.FindLastRoute(Enums.DataType.User).ElementData; } }

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    #region Properties
    private string IconPath
    {
        get { return UserElementData.IconPath; }
    }

    private string Username
    {
        get { return UserElementData.Username; }
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
        userIcon.texture = Resources.Load<Texture2D>(IconPath);
        usernameText.text = Username;
    }

    public void OpenSegment()
    {
        gameObject.SetActive(true);

    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
