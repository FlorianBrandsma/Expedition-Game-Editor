public interface ISegment
{
    SegmentController SegmentController { get; }

    IEditor DataEditor { get; }
    void InitializeDependencies();
    void InitializeSegment();
    void InitializeData();
    void OpenSegment();
    void CloseSegment();
    void SetSearchResult(SelectionElement selectionlement);
}
