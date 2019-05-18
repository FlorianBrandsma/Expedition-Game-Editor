public interface ISegment
{
    IEditor DataEditor { get; }
    void InitializeSegment();
    void OpenSegment();
    void ApplySegment();
    void CloseSegment();
    void SetSearchResult(SelectionElement selectionlement);
}
