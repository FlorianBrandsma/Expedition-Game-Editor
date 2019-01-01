﻿using UnityEngine;
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
        Chapter,
        ChapterSelection,
        PhaseSelection,
        QuestSelection,
        ObjectiveSelection,
        Task,
        Region,
        Terrain,
        TerrainSelection,
        Popup,
    }

    static public HistoryManager historyManager;

    private List<HistoryElement> history = new List<HistoryElement>();

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
            CloseForm(history[history.Count - 1].path.form);

            history.RemoveAt(history.Count - 1);

            OpenPath();
        }
    }

    public void CloseForm(EditorForm form)
    {
        form.CloseForm(true);
    }

    public void OpenPath()
    {
        EditorManager.editorManager.OpenPath(history[history.Count - 1].path);
    }
}
