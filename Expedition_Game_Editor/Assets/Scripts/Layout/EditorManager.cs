using UnityEngine;
using System.Collections.Generic;

public class Test
{
    public int value;

    public Test(int new_value)
    {
        value = new_value;
    }

    public Test(Test test)
    {
        value = test.value;
    }

    public Test Copy()
    {
        return new Test(this);
    }
}

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
        //TestTwo();
        
        path.section.InitializePath(path);
    }

    public void TestTwo()
    {
        Selection test1 = new Selection();
        Selection test2 = test1.Copy();

        test1.data.table = "Test1";
        test2.data.table = "Test2";

        Debug.Log(test2.data.table);
        Debug.Log(test1.data.table);
    }

    public void Test()
    {
        Test test1 = new Test(0);

        Test test2 = test1.Copy();

        //Test test2 = new Test(test1);

        test2.value = 5;

        Debug.Log(test2.value);
        Debug.Log(test1.value);
    }

    public void OpenPath(Path path)
    {
        path.section.OpenPath(path);
        /*
        foreach (HistoryElement element in HistoryManager.historyManager.history)
        {
            if (element.path.origin != null)
                Debug.Log(element.path.origin.data.table);          
        }  
        */
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



