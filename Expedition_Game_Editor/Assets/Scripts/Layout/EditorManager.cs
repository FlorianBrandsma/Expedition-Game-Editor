using UnityEngine;
using System.Collections.Generic;

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

        InitializePath(new PathManager.Primary().Initialize());
        InitializePath(new PathManager.Secondary().Initialize());
        InitializePath(new PathManager.Tertiary().Initialize());
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
    }

    public void InitializePath(Path path)
    {
        path.window.InitializePath(path);
    }

    public void OpenPath(Path path)
    {
        path.window.OpenPath(path);
    }

    public void PreviousEditor()
    {
        for (int i = windows.Length; i > 0; i--)
        {
            HistoryManager historyManager = windows[i - 1].GetComponent<HistoryManager>();

            if (historyManager.history.Count > historyManager.history_min)
            {
                historyManager.PreviousEditor();
                break;
            }
        }
    }

    public void ResetEditor()
    {
        windows[0].ResetPath();

        foreach (WindowManager window in windows)
            window.ResetPath();
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
        string str = "structure: ";

        for (int i = 0; i < path.structure.Count; i++)
            str += path.structure[i] + ",";

        str += "id: ";

        for (int i = 0; i < path.id.Count; i++)
            str += path.id[i] + ",";

        return str;
    }
}



