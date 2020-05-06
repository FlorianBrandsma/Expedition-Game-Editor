using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if (!GlobalManager.loaded)
        {
            GlobalManager.programType = GlobalManager.Scenes.Game;

            GlobalManager.OpenScene(GlobalManager.Scenes.Global);

            return;
        }
    }

    private void Start()
    {
        RenderManager.Render(new PathManager.Game().Initialize());
    }

    public void PreviousPath()
    {
        RenderManager.PreviousPath();
    }
}