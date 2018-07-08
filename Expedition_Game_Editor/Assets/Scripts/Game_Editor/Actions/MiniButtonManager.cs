using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MiniButtonManager : MonoBehaviour
{
    public int[]    select_path;
    public string   type;

    public void SetButtons()
    {
        Button button = GetComponent<EditorController>().actionManager.AddMiniButton();
        button.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture2D>("Textures/Icons/" + type);
    }
}
