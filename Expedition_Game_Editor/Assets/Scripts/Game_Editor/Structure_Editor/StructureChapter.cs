using UnityEngine;
using System.Collections;

public class StructureChapter : MonoBehaviour, IEditor
{
    public void OpenEditor()
    {
        gameObject.SetActive(true);
    }

    public void CloseEditor()
    {
        gameObject.SetActive(false);
    }
}
