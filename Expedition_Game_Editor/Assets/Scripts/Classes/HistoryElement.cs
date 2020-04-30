[System.Serializable]
public class HistoryElement
{
    public Path path { get; set; }

    public HistoryManager.Group group;

    public HistoryElement(HistoryElement historyElement)
    {
        path = historyElement.path;
        group = historyElement.group;
    }

    public void AddHistory(Path path)
    {
        this.path = path;
        RenderManager.historyManager.AddHistory(new HistoryElement(this));
    }
}
