using UnityEngine;

public class EditorManager : MonoBehaviour
{
    static public EditorManager editorManager;

    static public RectTransform UI;

    public EditorForm[] forms;

    private void Awake()
    {
        editorManager = this;

        UI = GetComponent<RectTransform>();

        foreach (EditorForm form in forms)
            form.InitializeForm();
    }

    private void Start()
    {
        LanguageManager.GetLanguage();

        InitializePath(new PathManager.Main().Initialize());

        //Debugging
        //InitializePath(new PathManager.Form(forms[1]).Initialize());
    }

    private void Update()
    {
        // Dirty fix: Unity dropdown closes with the same button as "previous" (hardcoded)
        // Closing a dropdown starts a "Fade" coroutine. 
        // Disabling the dropdown causes the fading list to get stuck.
        if (GameObject.Find("Dropdown List") != null) return;
        
        if (Input.GetKeyUp(KeyCode.Escape))
            PreviousEditor(); 
    }

    public void InitializePath(Path path)
    {
        path.form.InitializePath(path);
    }

    public void OpenPath(Path path)
    {
        SelectionManager.CancelGetSelection();

        path.form.OpenPath(path);

        SelectionManager.SelectElements();
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
        foreach (EditorForm form in forms)
            form.ResetPath();
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
            str += path.route[i].controller + ",";

        str += "id: ";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i].data.id + ",";

        return str;
    }
}



