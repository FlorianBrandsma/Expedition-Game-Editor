[System.Serializable]
public class HistoryElement
{
    public Path path { get; set; }

    public HistoryManager.Group group;

    public HistoryElement(HistoryElement historyElement)
    {
        path = historyElement.path.Copy();
        group = historyElement.group;
    }

    public void AddHistory(Path path)
    {
        this.path = path;
        EditorManager.historyManager.AddHistory(new HistoryElement(this));
    }
}
