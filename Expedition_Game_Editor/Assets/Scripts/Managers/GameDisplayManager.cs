using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplayManager : MonoBehaviour
{
    public enum Display
    {
        Game,
        Data,
    }

    static public Display activeDisplay = Display.Game;

    static public void SetDisplay(int display, Route route)
    {
        activeDisplay = (Display)display;

        var path = new PathManager.GameDisplay(route).Open();

        RenderManager.Render(path);
    }
}
