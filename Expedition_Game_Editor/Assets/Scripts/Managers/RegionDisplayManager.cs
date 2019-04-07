using UnityEngine;

public class RegionDisplayManager : MonoBehaviour
{
    public enum Display
    {
        Terrain,
        Tiles,
    }

    static public Display default_display = Display.Tiles;
    static public Display active_display;

    static public void GetDisplay()
    {
        active_display = 0;
    }

    static public void SetDisplay(int new_display, Path new_path)
    {
        active_display = (Display)new_display;

        EditorManager.editorManager.InitializePath(new_path);
    }

    static public void ResetDisplay()
    {
        active_display = default_display;
    }
}
