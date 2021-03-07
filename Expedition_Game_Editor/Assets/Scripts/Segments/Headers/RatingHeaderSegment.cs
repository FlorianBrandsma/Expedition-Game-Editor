using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RatingHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text headerText;

    public List<ExRatingStar> ratingStarList;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    private float Rating
    {
        get { return ((GameEditor)DataEditor).Rating; }
        set
        {
            var gameEditor = (GameEditor)DataEditor;
            gameEditor.Rating = value;
        }
    }

    private bool Installed
    {
        get { return ((GameEditor)DataEditor).Installed; }
    }

    private string IconPath
    {
        get { return ((GameEditor)DataEditor).IconPath; }
    }

    private string Title
    {
        get { return ((GameEditor)DataEditor).Name; }
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

    public void InitializeSegment()
    {
        SetStars();
    }

    public void OpenSegment()
    {
        icon.texture = Resources.Load<Texture2D>(IconPath);
        headerText.text = Title;
        
        gameObject.SetActive(true);
    }

    public void RateGame(int value)
    {
        if (!Installed) return;

        Rating = value;
        
        SetStars();
    }

    private void SetStars()
    {
        for (int i = 0; i < ratingStarList.Count; i++)
        {
            var ratingStar = ratingStarList[i];

            ratingStar.SetStar((Rating - i));
        }
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
