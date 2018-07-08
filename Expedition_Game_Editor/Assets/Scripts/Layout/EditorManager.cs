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

public class EditorManager : MonoBehaviour
{
    static public EditorManager editorManager;

    static public RectTransform UI;

    public WindowManager[] windows;

    private void Awake()
    {
        editorManager = this;

        UI = GetComponent<RectTransform>();

        foreach (WindowManager window in windows)
            window.InitializeWindow();
    }

    private void Start()
    {
        LanguageManager.GetLanguage();

        windows[0].InitializePath(new Path(new List<int> { 0,0 }, new List<int> { 0,0 }), true);
        windows[1].InitializePath(new Path(new List<int> { }, new List<int> { }), true);
    }

    private void Update()
    {
        // Dirty fix: Unity dropdown closes with the same button as "previous" (hardcoded)
        // Closing a dropdown starts a "Fade" coroutine. 
        // Disabling the dropdown causes the fading list to get stuck.
        if (GameObject.Find("Dropdown List") == null)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                PreviousEditor();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            windows[0].InitializePath(new Path(new List<int> { 0, 0, 1 }, new List<int> { 0, 0, 0 }), true);
            //windows[1].OpenPath(new Path(new List<int> { }, new List<int> { }), false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            windows[0].InitializePath(new Path(new List<int> { 0, 1, 0 }, new List<int> { 0, 0, 0 }), true);
            //windows[1].OpenPath(new Path(new List<int> { }, new List<int> { }), false);
        }

    }

    public void OpenEditor(Path path, bool previous)
    {

    }

    public void PreviousEditor()
    {
        for (int i = windows.Length; i > 0; i--)
        {
            if (windows[i - 1].history.Count > 1)
            {
                windows[i - 1].PreviousEditor();
                break;
            }
        }
    }

    public void ResetEditor()
    {
        windows[0].ResetPath();

        /*
        foreach (WindowManager window in windows)
            window.ResetPath();
*/
    }

    static public void SelectElement(int id)
    {
        //set_id = id;
    }

    static public void ResetSelection()
    {
        //get_id = false;
        //set_id = 0;
    }

    static public string PathString(Path path)
    {
        string str = "editor:";

        for (int i = 0; i < path.editor.Count; i++)
            str += path.editor[i] + ",";

        return str;
    }
}
