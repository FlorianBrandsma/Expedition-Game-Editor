using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text headerText;

    public List<ExRatingStar> ratingStarList;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    private string Title
    {
        get { return ((GameEditor)DataEditor).Name; }
    }

    private string IconPath
    {
        get { return ((GameEditor)DataEditor).IconPath; }
    }

    private int Rating
    {
        get { return ((GameEditor)DataEditor).Rating; }
        set
        {
            var gameEditor = (GameEditor)DataEditor;
            gameEditor.Rating = value;
        }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        icon.texture = Resources.Load<Texture2D>(IconPath);
        headerText.text = Title;
        
        gameObject.SetActive(true);
    }

    public void RateGame(int value)
    {

        //Set stars
    }

    private void SetStars()
    {

    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}

