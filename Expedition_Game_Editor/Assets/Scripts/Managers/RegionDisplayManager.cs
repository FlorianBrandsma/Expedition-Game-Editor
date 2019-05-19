using UnityEngine;

public class RegionDisplayManager : MonoBehaviour
{
    public enum Display
    {
        Terrain,
        Tiles,
    }

    static public Display default_display = Display.Tiles;
    static public Display activeDisplay;

    static public void GetDisplay()
    {
        activeDisplay = 0;
    }

    static public void SetDisplay(int new_display, Path new_path)
    {
        activeDisplay = (Display)new_display;

        EditorManager.editorManager.InitializePath(new_path);
    }

    static public void ResetDisplay()
    {
        activeDisplay = default_display;
    }
}
