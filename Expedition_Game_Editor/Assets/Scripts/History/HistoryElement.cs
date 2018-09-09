using UnityEngine;
using System.Collections;

public class HistoryElement : MonoBehaviour
{
    public Path path
    {
        get { return GetComponent<EditorController>().path; }
        set { }
    }

    public HistoryManager historyManager;

    public HistoryManager.Group group;

    public void AddHistory()
    {
        historyManager.AddHistory(this);
    }
}
