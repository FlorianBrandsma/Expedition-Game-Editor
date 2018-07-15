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

    public void AddHistory(Path path)
    {
        if (history.Count > 0 && path.editor.Count == history[history.Count - 1].editor.Count)
            history[history.Count - 1] = path;
        else
            history.Add(path);     
    }

    public void PreviousEditor()
    {
        if(history.Count > 1 || sibling_historyManager != null)
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
        GetComponent<WindowManager>().InitializePath(history[history.Count - 1]);
    }
}
