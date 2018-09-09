using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HistoryManager : MonoBehaviour
{
    public enum Group
    {
        None,
        Assets,
        ChapterSelection,
        PhaseSelection,
        QuestSelection,
        ObjectiveSelection,
        Region,
        Terrain,
        TerrainSelection,
        Popup,
    }

    public List<HistoryElement> history = new List<HistoryElement>();

    public HistoryManager sibling_historyManager;

    public int history_min;

    bool previous;

    public void AddHistory(HistoryElement element)
    {
        if (history.Count > 0 && element.group == history[history.Count - 1].group)
        {
            //Debug.Log("Replace history");
            history[history.Count - 1] = element;
        } else {
            //Debug.Log("Add history");
            history.Add(element);
        }

        if (!previous)
        {
            //Debug.Log("Add this to history");
            

            previous = false;
        } 
    }

    public void PreviousEditor()
    {
        if(history.Count > history_min || sibling_historyManager != null)
        {
            previous = true;

            history.RemoveAt(history.Count - 1);

            if (history.Count > 0)
                InitializePath();
            else if (sibling_historyManager != null)
                sibling_historyManager.InitializePath();
        }
    }
    public void InitializePath()
    {
        GetComponent<SectionManager>().OpenPath(history[history.Count - 1].path);
    }
}
