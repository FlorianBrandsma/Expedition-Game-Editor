using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Path
{
    public List<int> editor { get; set; }
    public List<int> id     { get; set; }

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
    private Path tempory_history    = new Path(new List<int>(), new List<int>());
    private Path source_history     = new Path(new List<int>(), new List<int>());

    private Path active_path        = new Path(new List<int>(), new List<int>());

    public SubEditor default_editor;
    public SubEditor source_editor;

    static public NavigationManager navigation_manager;

    private void Awake()
    {
        navigation_manager = GetComponent<NavigationManager>();
    }

    private void Start()
    {
        LanguageManager.GetLanguage();

        OpenStructure(  new Path(new List<int>() { 0,0 }, new List<int>() { 0,0 }), false, false);
        OpenSource(     new Path(new List<int>() {  },  new List<int>() {  }));    
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && history.Count > 1)
            PreviousEditor();    
    }

    public void OpenStructure(Path path, bool temporary, bool previous)
    {
        CloseEditor(active_path, default_editor);

        active_path = path;

        if (!previous && tempory_history.editor.Count == 0)
            history.Add(path);

        if (temporary)
            tempory_history = path;
        else
            tempory_history = new Path(new List<int>(), new List<int>());

        if (previous)
            history.RemoveAt(history.Count - 1);

        if (path.editor.Count > 0)
        {
            default_editor.OpenEditor(path, 0);
        }
    }

    public void PreviousEditor()
    {
        // Dirty fix: Unity dropdown closes with the same button as "previous" (hardcoded)
        // Closing a dropdown starts a "Fade" coroutine. 
        // Disabling the dropdown causes the fading list to get stuck.

        if (GameObject.Find("Dropdown List") == null)
            OpenStructure(history[history.Count - 2], false, true);  
    }

    //Open temporary window
    //Does not close -!any!- other editors

    public void OpenSource(Path path)
    {
        CloseEditor(source_history, source_editor);

        source_editor.OpenEditor(path, 0);

        source_history = path;
    }

    void CloseEditor(Path path, SubEditor base_editor)
    {
        ResetSelection();

        if (path.editor.Count > 0)
            base_editor.CloseEditor(path.editor, 0); 
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

    public void RefreshStructure()
    {
        CloseEditor(active_path, default_editor);

        default_editor.OpenEditor(active_path, 0);
    }

    public void RefreshSource()
    {
        OpenSource(source_history);
    }
}
