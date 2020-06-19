using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

static public class HistoryManager
{
    public enum Group
    {
        None,
        Assets,
        Stage,
        Chapter,
        ChapterSelection,
        Phase,
        PhaseSelection,
        Quest,
        QuestSelection,
        Objective,
        ObjectiveSelection,
        Interactable,
        InteractableSelection,
        Task,
        TaskSelection,
        Interaction,
        InteractionSelection,
        Region,
        Terrain,
        TerrainSelection,
        Popup,
        Atmosphere,
        Outcome,
        OutcomeSelection,
        Menu,
        MenuSelection
    }

    static private List<HistoryElement> history = new List<HistoryElement>();

    static public void AddHistory(HistoryElement element)
    {
        if (history.Count > 0 && element.group == history[history.Count - 1].group)
        {
            history[history.Count - 1] = element;
        } else {
            history.Add(element);
        }
    }

    static public void ClearHistory()
    {
        history.Clear();
    }

    static public void PreviousPath()
    {
        if(history.Count > 1)
        {
            var historyForm = history[history.Count - 1].path.form;
            
            history.RemoveAt(history.Count - 1);

            var newHistoryForm = history[history.Count - 1].path.form;

            if (historyForm == newHistoryForm)
                InitializePath();
            else
                CloseForm(historyForm);
        }
    }

    static public void CloseForm(EditorForm form)
    {
        form.CloseForm();
    }

    static public void InitializePath()
    {
        RenderManager.loadType = Enums.LoadType.Return;

        RenderManager.ResetPath(history[history.Count - 1].path);
    }
}
