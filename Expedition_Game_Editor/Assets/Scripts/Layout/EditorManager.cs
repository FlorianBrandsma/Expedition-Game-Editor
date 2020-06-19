using UnityEngine;

public class EditorManager : MonoBehaviour
{
    private void Awake()
    {
        if (!GlobalManager.loaded)
        {
            GlobalManager.programType = GlobalManager.Scenes.Editor;

            GlobalManager.OpenScene(GlobalManager.Scenes.Global);

            return;
        }
    }

    private void Start()
    {
        RenderManager.Render(new PathManager.Editor().Initialize());
    }

    public void PreviousPath()
    {
        RenderManager.PreviousPath();
    }
}