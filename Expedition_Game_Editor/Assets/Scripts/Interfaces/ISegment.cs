public interface ISegment
{
    IEditor DataEditor { get; set; }
    void InitializeSegment();
    void OpenSegment();
    void ApplySegment();
    void CloseSegment();
    void SetSearchResult(SelectionElement selectionlement);
}
