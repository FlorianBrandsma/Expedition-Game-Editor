public interface ISegment
{
    IEditor dataEditor { get; set; }
    void InitializeSegment();
    void OpenSegment();
    void ApplySegment();
    void CloseSegment();
    void SetSearchResult(SearchElement searchElement);
}
