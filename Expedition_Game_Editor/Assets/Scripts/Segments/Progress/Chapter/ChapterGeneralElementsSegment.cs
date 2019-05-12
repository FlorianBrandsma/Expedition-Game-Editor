using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterGeneralElementsSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

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
        if (SegmentController.editorController.pathController.loaded) return;

        ChapterDataElement chapterData = DataEditor.data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        var searchParameters = new Search.Element();

        searchParameters.requestType = Search.Element.RequestType.Custom;
        searchParameters.includedIdList = chapterData.elementIds;
        searchParameters.temp_id_count = 4;

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    private void SetSearchParameters()
    {
        ChapterDataElement chapterData = DataEditor.data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();
        var searchParameters = SegmentController.DataController.SearchParameters.Cast<SearchParameters>().FirstOrDefault();

        searchParameters.exclusedIdList = chapterData.elementIds;
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        

        ChapterDataElement chapterData = DataEditor.data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        chapterData.ElementIds = SegmentController.DataController.DataList.Cast<ElementDataElement>().Select(x => x.id).ToList();

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
