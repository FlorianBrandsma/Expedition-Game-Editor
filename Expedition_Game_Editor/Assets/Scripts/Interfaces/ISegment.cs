public interface ISegment
{
    SegmentController SegmentController { get; }
    IEditor DataEditor                  { get; }

    void InitializeDependencies();
    void InitializeData();
    void InitializeSegment();
    void OpenSegment();
    void UpdateSegment();
    void SetSearchResult(IElementData searchData, IElementData resultData);
    void CloseSegment();
}
