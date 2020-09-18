using UnityEngine;

public class ChapterGeneralTimeSpeedSegment : MonoBehaviour, ISegment
{
    public ExInputNumber timeSpeedInputNumber;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public ChapterEditor ChapterEditor          { get { return (ChapterEditor)DataEditor; } }

    #region Data properties
    private float TimeSpeed
    {
        get { return ChapterEditor.TimeSpeed; }
        set { ChapterEditor.TimeSpeed = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        timeSpeedInputNumber.Value = TimeSpeed;
    }
    
    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateTimeSpeed()
    {
        TimeSpeed = timeSpeedInputNumber.Value;

        DataEditor.UpdateEditor();
    }
    
    public void CloseSegment() { }
}
