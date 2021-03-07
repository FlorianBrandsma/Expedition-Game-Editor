using UnityEngine;
using UnityEngine.UI;

public class TeamHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage userIcon;
    public Text usernameText;

    private TeamElementData TeamElementData         { get { return (TeamElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Team).ElementData; } }

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    #region Properties
    private string IconPath
    {
        get { return TeamElementData.IconPath; }
    }

    private string Name
    {
        get { return TeamElementData.Name; }
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
        usernameText.text = Name;
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
