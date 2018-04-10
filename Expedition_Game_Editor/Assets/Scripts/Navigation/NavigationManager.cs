using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Path
{
    public List<int> editor { get; set; }
    public List<int> id { get; set; }

    public Path(List<int> new_editor, List<int> new_id)
    {
        editor = new_editor;
        id = new_id;
    }

    public void Clear()
    {
        editor.Clear();
        id.Clear();
    }
}

public class NavigationManager : MonoBehaviour
{
    //SelectionManager?
    static public bool get_id;
    static public int  set_id;

    static public List<Path> history = new List<Path>();
    private Path tempory_history = new Path(new List<int>(), new List<int>());
    private Path source_history = new Path(new List<int>(), new List<int>());

    private Path active_path = new Path(new List<int>(), new List<int>());

    public GameObject[] editor;

    static public NavigationManager navigation_manager;

    private void Awake()
    {
        navigation_manager = GetComponent<NavigationManager>();
    }

    private void Start()
    {
        OpenEditor(new Path(new List<int>() { 1 }, new List<int>() { 0 }), false, false);
        OpenSource(new Path(new List<int>() { 0, 4 }, new List<int>() { 0, 0 }));
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && history.Count > 1)
            PreviousEditor();
    }

    public void OpenEditor(Path path, bool temporary, bool previous)
    {
        if(active_path != null)
            CloseEditor(active_path, 0);

        active_path = path;

        if (!previous && tempory_history.editor.Count == 0)
            history.Add(active_path);

        if (temporary)
            tempory_history = active_path;
        else
            tempory_history = new Path(new List<int>(), new List<int>());

        if (previous)
            history.RemoveAt(history.Count - 1);

        if (path.editor.Count > 0)
            editor[path.editor[0]].GetComponent<SubEditor>().OpenEditor(active_path, 0);
    }

    public void PreviousEditor()
    {
        OpenEditor(history[history.Count - 2], false, true);  
    }

    public void OpenSource(Path path)
    {
        CloseEditor(source_history, 0);

        editor[path.editor[0]].GetComponent<SubEditor>().OpenEditor(path, 0);

        source_history = path;
    }

    void CloseEditor(Path path, int editor_index)
    {
        ResetSelection();

        if (path.editor.Count > 0)
            editor[path.editor[0]].GetComponent<SubEditor>().CloseEditor(path.editor, 0);
    }

    static public void SelectElement(int id)
    {
        set_id = id;
    }

    static public void ResetSelection()
    {
        get_id = false;
        set_id = 0;
    }
}
