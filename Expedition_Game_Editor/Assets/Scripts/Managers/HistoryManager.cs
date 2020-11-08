using UnityEngine;
using System.Collections.Generic;

static public class HistoryManager
{
    static private List<Path> historyPath = new List<Path>();

    static public void AddHistory(Path path)
    {
        if (path.historyGroup == Enums.HistoryGroup.None) return;

        if (historyPath.Count > 0 && path.historyGroup == historyPath[historyPath.Count - 1].historyGroup)
        {
            historyPath[historyPath.Count - 1] = path;

        } else {

            historyPath.Add(path);
        }
    }

    static public void ClearHistory()
    {
        historyPath.Clear();
    }

    static public void PreviousPath()
    {
        if(historyPath.Count > 1)
        {
            var historyForm = historyPath[historyPath.Count - 1].form;
            
            historyPath.RemoveAt(historyPath.Count - 1);

            var newHistoryForm = historyPath[historyPath.Count - 1].form;

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

        RenderManager.ResetPath(historyPath[historyPath.Count - 1]);
    }
}
