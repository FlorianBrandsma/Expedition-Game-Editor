using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class HistoryManager
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
        OutcomeSelection
    }

    private List<HistoryElement> history = new List<HistoryElement>();

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
            var historyForm = history[history.Count - 1].path.form;
            
            history.RemoveAt(history.Count - 1);

            var newHistoryForm = history[history.Count - 1].path.form;

            if (historyForm == newHistoryForm)
                InitializePath();
            else
                CloseForm(historyForm);
        }
    }

    public void CloseForm(EditorForm form)
    {
        form.CloseForm();
    }

    public void InitializePath()
    {
        EditorManager.loadType = Enums.LoadType.Return;

        EditorManager.editorManager.ResetEditor(history[history.Count - 1].path);
    }
}
