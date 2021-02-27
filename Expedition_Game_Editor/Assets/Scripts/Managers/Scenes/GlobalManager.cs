using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public enum Scenes
    {
        Global,
        Main,
        Editor,
        Game
    }

    static public bool loaded;

    static public Scenes programType = Scenes.Main;

    private void Awake()
    {
        loaded = true;

        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        InitializeScene();
    }

    private void Update()
    {
        //Escape button shares a built in function of the dropdown that closes it
        if (GameObject.Find("Dropdown List") != null) return;

        if (Input.GetKeyUp(KeyCode.Escape))
            RenderManager.PreviousPath();
    }

    private void InitializeScene()
    {
        SceneManager.LoadScene((int)programType);
    }
    
    static public void OpenScene(Scenes scene)
    {
        RenderManager.CloseForms();

        HistoryManager.ClearHistory();
        
        SceneManager.LoadScene((int)scene);
    }

    static public void CloseApplication()
    {
        Application.Quit();
    }
}
