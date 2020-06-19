using UnityEngine;
using System.Collections;

public class EditorLayout : MonoBehaviour
{
    public LayoutAnchors layoutSection;

    public Vector2  anchor_min,
                    anchor_max;

    public void InitializeLayout()
    {
        layoutSection.InitializeLayout(anchor_min, anchor_max);
    }

    public void CloseLayout()
    {
        layoutSection.CloseSection();
    }
}
