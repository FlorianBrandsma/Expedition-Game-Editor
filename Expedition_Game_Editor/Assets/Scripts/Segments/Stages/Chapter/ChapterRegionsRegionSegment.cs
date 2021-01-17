using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionsRegionSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private ChapterEditor ChapterEditor         { get { return (ChapterEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.ChapterRegion);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);

        var chapterRegionList = SegmentController.DataController.Data.dataList.Cast<ChapterRegionElementData>().ToList();
        ChapterEditor.chapterRegionElementDataList = chapterRegionList.Where(x => x.Id != -1).ToList();
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterRegion>().First();

        searchParameters.chapterId = new List<int>() { ChapterEditor.Id };

        searchParameters.includeAddElement = true;
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        SetSearchParameters();
        
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    private void SetSearchParameters()
    {
        var searchParameters = SegmentController.DataController.SearchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.excludeId  = ChapterEditor.chapterRegionElementDataList.Select(x => x.RegionId).Distinct().ToList();
        searchParameters.phaseId    = new List<int>() { 0 };

        searchParameters.includeRemoveElement = true;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        SetSearchChapterRegion((ChapterRegionElementData)mergedElementData, resultElementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchChapterRegion(ChapterRegionElementData mergedElementData, IElementData resultElementData)
    {
        var searchElementData = (ChapterRegionElementData)SegmentController.DataController.Data.dataList.Where(x => x.Id == mergedElementData.Id).First();

        if (searchElementData.Id == -1)
        {
            searchElementData = (ChapterRegionElementData)searchElementData.Clone();

            searchElementData.ExecuteType = Enums.ExecuteType.Add;

            searchElementData.Id = -(ChapterEditor.chapterRegionElementDataList.Count + 2);

            searchElementData.ChapterId     = ChapterEditor.EditData.Id;

            searchElementData.RegionId      = mergedElementData.RegionId;
            searchElementData.Name          = mergedElementData.Name;
            searchElementData.TileIconPath  = mergedElementData.TileIconPath;

            SegmentController.DataController.Data.dataList.Add(searchElementData);
            ChapterEditor.chapterRegionElementDataList.Add(searchElementData);
        }

        if (resultElementData.Id == 0)
        {
            searchElementData.ExecuteType = Enums.ExecuteType.Remove;

            SegmentController.DataController.Data.dataList.Remove(searchElementData);
        }
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
