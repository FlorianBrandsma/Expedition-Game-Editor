using UnityEngine;
using System.Linq;

public class ChapterGeneralTimeSpeedSegment : MonoBehaviour, ISegment
{
    public ExInputNumber timeSpeedInputNumber;

    private float timeSpeed;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public float TimeSpeed
    {
        get { return timeSpeed; }
        set
        {
            timeSpeed = value;

            var chapterDataList = DataEditor.DataList.Cast<ChapterElementData>().ToList();
            chapterDataList.ForEach(chapterData =>
            {
                chapterData.TimeSpeed = timeSpeed;
            });
        }
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var chapterData = (ChapterElementData)DataEditor.ElementData;

        timeSpeed = chapterData.TimeSpeed;
    }

    public void InitializeSegment()
    {
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        timeSpeedInputNumber.Value = timeSpeed;
    }

    public void UpdateTimeSpeed()
    {
        TimeSpeed = timeSpeedInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
