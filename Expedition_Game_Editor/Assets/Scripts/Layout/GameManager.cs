using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public EditorManager instance;
    
    private void Awake()
    {
        if (!GlobalManager.loaded)
        {
            GlobalManager.programType = GlobalManager.ProgramType.Game;

            SceneManager.LoadScene("Global");
            return;
        }
    }

    private void Start()
    {
        RenderManager.Render(new PathManager.Main().Initialize());
    }

    public void PreviousPath()
    {
        RenderManager.PreviousPath();
    }
}
