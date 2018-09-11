using UnityEngine;
using System.Collections.Generic;

public class EditorManager : MonoBehaviour
{
    static public EditorManager editorManager;

    static public RectTransform UI;

    public SectionManager[] sections;

    private void Awake()
    {
        editorManager = this;

        UI = GetComponent<RectTransform>();

        foreach (SectionManager section in sections)
            section.InitializeSection();
    }

    private void Start()
    {
        LanguageManager.GetLanguage();

        InitializePath(new PathManager.Secondary().Initialize());
        InitializePath(new PathManager.Primary().Initialize());
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
        path.section.InitializePath(path);
    }

    public void OpenPath(Path path)
    {
        path.section.OpenPath(path);
    }

    public void ResetPath()
    {

    }

    public void PreviousEditor()
    {
        HistoryManager.historyManager.PreviousEditor();  
    }

    public void ResetEditor()
    {
        foreach (SectionManager section in sections)
            section.ResetPath();
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
        string str = "route: ";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i] + ",";

        str += "id: ";

        for (int i = 0; i < path.data.Count; i++)
            str += path.data[i].id + ",";

        return str;
    }
}



