using UnityEngine;

public class GameDisplayManager : MonoBehaviour
{
    public enum Display
    {
        Game,
        Data,
    }

    static public Display activeDisplay = Display.Game;

    static public void SetDisplay(int display, Path path)
    {
        activeDisplay = (Display)display;

        HistoryManager.ClearHistory();

        RenderManager.Render(path.Clone());
    }
}
