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

    public void AddHistory(Path new_path)
    {
        path = new_path.Copy();
        HistoryManager.historyManager.AddHistory(new HistoryElement(this));
    }
}
