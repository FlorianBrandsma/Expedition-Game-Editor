using UnityEngine;
using System.Collections.Generic;

//    [UnityEngine.Serialization.FormerlySerializedAs("previous_name")]

public class EditorManager : MonoBehaviour
{
    static public EditorManager editorManager;

    static public RectTransform UI;

    public EditorForm[] forms;

    static public HistoryManager historyManager = new HistoryManager();
    //static public PoolManager poolManager = new PoolManager();

    private void Awake()
    {
        Fixtures.LoadFixtures();
        //Fixtures.CalculateFixtures();

        editorManager = this;

        UI = GetComponent<RectTransform>();
        
        foreach (EditorForm form in forms)
            form.InitializeForm();
    }

    private void Start()
    {
        LanguageManager.GetLanguage();

        InitializePath(new PathManager.Main().Initialize());
    }

    private void Update()
    {
        //Escape button shares a built in function of the dropdown that closes it
        if (GameObject.Find("Dropdown List") != null) return;
        
        if (Input.GetKeyUp(KeyCode.Escape))
            PreviousEditor();
        //if (Input.GetKeyUp(KeyCode.Space))
        //    ResetEditor();
    }

    public void InitializePath(Path path, bool returning = false)
    {
        historyManager.returned = returning;

        SelectionManager.CancelGetSelection();

        path.form.InitializePath(path);

        SelectionManager.SelectElements();
    }

    public void PreviousEditor()
    {
        historyManager.PreviousEditor();  
    }

    public void ResetEditor()
    {
        foreach (EditorForm form in forms)
            form.ResetPath();

        SelectionManager.SelectElements();
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
            str += path.route[i].controller + "/";

        str += ", id: ";

        for (int i = 0; i < path.route.Count; i++)
            str += path.route[i].GeneralData().table + "-" + path.route[i].GeneralData().id + "/";

        return str;
    }
}



