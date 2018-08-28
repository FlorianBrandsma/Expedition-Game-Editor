using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HistoryManager : MonoBehaviour
{
    public List<Path> history = new List<Path>();

    public HistoryManager sibling_historyManager;

    public int history_min;

    public void AddHistory(Path path)
    {
        if (history.Count > 0 && path.route.Count == history[history.Count - 1].route.Count)
            history[history.Count - 1] = path;
        else
            history.Add(path);        
    }

    public void PreviousEditor()
    {
        if(history.Count > history_min || sibling_historyManager != null)
        {
            history.RemoveAt(history.Count - 1);

            if (history.Count > 0)
                InitializePath();
            else if (sibling_historyManager != null)
                sibling_historyManager.InitializePath();
        }
    }
    public void InitializePath()
    {
        GetComponent<WindowManager>().OpenPath(history[history.Count - 1]);
    }
}
