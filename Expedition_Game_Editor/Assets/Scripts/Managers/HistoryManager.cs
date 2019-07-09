﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HistoryManager
{
    public enum Group
    {
        None,
        Assets,
        Progress,
        Chapter,
        ChapterSelection,
        Phase,
        PhaseSelection,
        Quest,
        QuestSelection,
        Step,
        StepSelection,
        Element,
        ElementSelection,
        Task,
        TaskSelection,
        Region,
        Terrain,
        TerrainSelection,
        Popup,
    }

    private List<HistoryElement> history = new List<HistoryElement>();

    public bool returned { get; set; }

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

            var historyForm = history[history.Count - 1].path.form;

            history.RemoveAt(history.Count - 1);

            if(historyForm == history[history.Count - 1].path.form)
                InitializePath();
        }
    }

    public void CloseForm(EditorForm form)
    {
        form.CloseForm();
    }

    public void InitializePath()
    {
        EditorManager.editorManager.InitializePath(history[history.Count - 1].path, true);
    }
}
