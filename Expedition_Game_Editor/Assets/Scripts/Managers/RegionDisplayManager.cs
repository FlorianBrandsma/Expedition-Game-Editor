﻿using UnityEngine;

public class RegionDisplayManager : MonoBehaviour
{
    public enum Display
    {
        Scene,
        Tiles,
    }

    static public Display activeDisplay = Display.Tiles;

    static public void SetDisplay(int display, Path path)
    {
        activeDisplay = (Display)display;

        EditorManager.editorManager.InitializePath(path);
    }
}
