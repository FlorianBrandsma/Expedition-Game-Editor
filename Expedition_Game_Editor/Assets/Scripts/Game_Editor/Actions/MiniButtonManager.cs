﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MiniButtonManager : MonoBehaviour
{
    public int[]    select_path;
    public string   type;

    public WindowManager windowManager;

    public void SetButtons()
    {
        Button button = GetComponent<EditorController>().actionManager.AddMiniButton();
        button.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture2D>("Textures/Icons/" + type);

        button.onClick.AddListener(delegate { OpenPath();});
    }

    void OpenPath()
    {
        Path path = new Path(new List<int>(), new List<int>());

        windowManager.OpenPath(path.CreateEditorPath(select_path));
    }
}
