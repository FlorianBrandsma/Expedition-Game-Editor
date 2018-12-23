using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public enum Display
    {
        List,
        Diagram,
    }

    static public Display default_display = Display.List;
    static public Display active_display;

    static public void GetDisplay()
    {
        active_display = 0;
    }

    static public void SetDisplay(int new_display)
    {
        active_display = (Display)new_display;

        EditorManager.editorManager.ResetEditor();
    }
}
