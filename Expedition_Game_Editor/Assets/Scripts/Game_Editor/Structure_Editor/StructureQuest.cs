using UnityEngine;
using System.Collections;

public class StructureQuest : MonoBehaviour, IEditor
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
