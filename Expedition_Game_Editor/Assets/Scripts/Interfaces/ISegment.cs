public interface ISegment
{
    SegmentController SegmentController { get; }

    IEditor DataEditor { get; }
    void InitializeDependencies();
    void InitializeData();
    void InitializeSegment();
    void OpenSegment();
    void CloseSegment();
    void SetSearchResult(DataElement dataElement);
}
