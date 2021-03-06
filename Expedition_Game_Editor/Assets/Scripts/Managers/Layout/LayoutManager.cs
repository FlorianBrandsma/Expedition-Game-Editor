﻿using UnityEngine;
using System.Collections.Generic;

public class LayoutManager : MonoBehaviour
{
    public RectTransform UI { get { return GetComponent<RectTransform>(); } }

    public List<EditorForm> forms;
    public List<EditorLayer> layers;

    private void Awake()
    {
        RenderManager.layoutManager = this;

        forms.ForEach(x => x.InitializeForm());
        layers.ForEach(x => x.InitializeAnchors());   
    }
}
