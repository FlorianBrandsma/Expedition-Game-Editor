public interface ISegment
{
    IEditor DataEditor { get; }
    void InitializeDependencies();
    void InitializeSegment();
    void InitializeData();
    void OpenSegment();
    void CloseSegment();
    void SetSearchResult(SelectionElement selectionlement);
}
