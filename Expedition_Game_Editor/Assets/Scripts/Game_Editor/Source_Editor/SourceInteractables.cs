using UnityEngine;
using System.Collections;

public class SourceInteractables : MonoBehaviour, IEditor
{
    public void OpenEditor()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseEditor()
    {
        //this.GetComponent<SubEditor>().CloseTabs();
        this.gameObject.SetActive(false);
    }
}