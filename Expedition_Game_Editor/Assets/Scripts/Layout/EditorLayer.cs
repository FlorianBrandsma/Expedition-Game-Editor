using UnityEngine;
using System.Collections.Generic;

public class EditorLayer : MonoBehaviour
{
    public List<LayoutAnchors> layoutSections;

    public List<EditorLayer> subLayers;

    public void InitializeAnchors()
    {
        layoutSections.ForEach(x => x.InitializeAnchors());
    }

    public void SetLayout()
    {
        layoutSections.ForEach(x => x.SetLayout());
    }
}
