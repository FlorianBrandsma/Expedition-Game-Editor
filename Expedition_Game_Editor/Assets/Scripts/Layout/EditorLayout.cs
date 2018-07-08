using UnityEngine;
using System.Collections;

public class EditorLayout : MonoBehaviour
{
    public LayoutManager layoutManager;

    public Vector2  anchor_min,
                    anchor_max;

    public bool overwrite;

    public void InitializeLayout()
    {
        layoutManager.InitializeLayout(anchor_min, anchor_max, overwrite);
    }

    public void SetLayout()
    {
        layoutManager.SetLayout();
    }

    public void CloseLayout()
    {
        layoutManager.CloseLayout();
    }
}
