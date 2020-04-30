using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorManager : MonoBehaviour
{
    static public EditorManager instance;
    
    private void Awake()
    {
        instance = this;

        if (!GlobalManager.loaded)
        {
            GlobalManager.programType = GlobalManager.ProgramType.Editor;

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



