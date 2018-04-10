using UnityEngine;
using System.Collections;

public class StructureChapter : MonoBehaviour, IEditor
{
    public void OpenEditor()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseEditor()
    {
        this.gameObject.SetActive(false);
    }
}
