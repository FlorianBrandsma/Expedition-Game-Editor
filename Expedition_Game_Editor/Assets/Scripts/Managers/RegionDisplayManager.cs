using UnityEngine;

static public class RegionDisplayManager
{
    public enum Display
    {
        World,
        Tiles,
    }

    static public Display activeDisplay;

    static public void SetDisplay(int display, Path path)
    {
        activeDisplay = (Display)display;

        RenderManager.Render(path);
    }
}
