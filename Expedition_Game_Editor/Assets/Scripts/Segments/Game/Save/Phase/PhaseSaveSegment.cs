using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PhaseSaveSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.PhaseSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseSave>().First();

        var chapterSaveElementData = (ChapterSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.ChapterSave).ElementData;
        searchParameters.chapterId = new List<int>() { chapterSaveElementData.ChapterId };

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
