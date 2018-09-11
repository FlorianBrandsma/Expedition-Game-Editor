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

    static public HistoryManager historyManager;

    public List<HistoryElement> history = new List<HistoryElement>();

    bool previous;

    private void Awake()
    {
        historyManager = this;
    }

    public void AddHistory(HistoryElement element)
    {
        if (history.Count > 0 && element.group == history[history.Count - 1].group)
        {
            history[history.Count - 1] = element;
        } else {
            history.Add(element);
        }
    }

    public void PreviousEditor()
    {
        if(history.Count > 1)
        {
            //previous = true;

            CloseSection(history[history.Count - 1].path.section);

            history.RemoveAt(history.Count - 1);

            InitializePath();
        }
    }

    public void CloseSection(SectionManager section)
    {
        section.CloseSection();
    }

    public void InitializePath()
    {
        EditorManager.editorManager.OpenPath(history[history.Count - 1].path);
    }
}
