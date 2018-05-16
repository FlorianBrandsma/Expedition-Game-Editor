using UnityEngine;
using System.Collections;

public class TileEditor : MonoBehaviour, IEditor
{
    public void OpenEditor(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void CloseEditor()
    {
        gameObject.SetActive(false);
    }
}
