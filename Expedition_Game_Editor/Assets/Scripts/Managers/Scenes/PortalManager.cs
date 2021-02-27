using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private void Awake()
    {
        if (!GlobalManager.loaded)
        {
            Fixtures.LoadFixtures();

            GlobalManager.programType = GlobalManager.Scenes.Main;

            GlobalManager.OpenScene(GlobalManager.Scenes.Global);

            return;
        }
    }

    private void Start()
    {
        RenderManager.Render(new PathManager.Portal().Initialize());
    }

    public void PreviousPath()
    {
        RenderManager.PreviousPath();
    }
}
