using UnityEngine;
using System.Collections;
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
        
        InitializeChapterData();
    }

    private void InitializeChapterData()
    {
        ChapterDataElement chapterData = DataEditor.data.element.Cast<ChapterDataElement>().FirstOrDefault();

        SegmentController.DataController.GetData(chapterData.elementIds);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().SetDisplay();
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
