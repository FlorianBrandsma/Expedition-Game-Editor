using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterGeneralElementsSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private ElementController ElementController { get { return (ElementController)SegmentController.DataController; } }

    public void ApplySegment()
    {
        
    }

    public void CloseSegment()
    {
        
    }

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
        
        InitializeElementData();

        SetSearchParameters();
    }

    private void InitializeElementData()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        if (chapterEditor.terrainElementDataList.Count > 0) return;

        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        var searchParameters = new Search.TerrainElement();

        searchParameters.requestType = Search.TerrainElement.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });
        
        var chapterElementList = SegmentController.DataController.DataList.Cast<TerrainElementDataElement>().ToList();

        chapterElementList.ForEach(x => chapterEditor.terrainElementDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();
        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        //Out of all the elements, select only those that are not in this list
        var idList = SegmentController.DataController.DataList.Cast<TerrainElementDataElement>().Select(x => x.ElementId).ToList();
        idList.Add(chapterData.ElementId);

        var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
