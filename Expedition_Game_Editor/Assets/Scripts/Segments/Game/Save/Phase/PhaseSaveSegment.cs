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
        searchParameters.chapterSaveId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.ChapterSave).ElementData.Id };

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

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
