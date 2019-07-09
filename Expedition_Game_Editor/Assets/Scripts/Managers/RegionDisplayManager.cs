using UnityEngine;

public class RegionDisplayManager : MonoBehaviour
{
    public enum Display
    {
        Terrain,
        Tiles,
    }

    static public Display activeDisplay = Display.Tiles;

    static public void GetDisplay()
    {
        activeDisplay = 0;
    }

    static public void SetDisplay(int display, Path path)
    {
        activeDisplay = (Display)display;

        EditorManager.editorManager.InitializePath(path);
    }
}
