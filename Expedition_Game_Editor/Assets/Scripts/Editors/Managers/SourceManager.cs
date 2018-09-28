using UnityEngine;
using System.Collections;

public class SourceManager : MonoBehaviour, IEditor
{
    public void OpenEditor()
    {
        gameObject.SetActive(true);
    }

    public void SaveEdit()
    {
        ApplyEdit();
        CancelEdit();
    }

    public void ApplyEdit()
    {

    }

    public void CancelEdit()
    {
        //NavigationManager.navigation_manager.PreviousEditor();
    }

    public void CloseEditor()
    {
        //gameObject.SetActive(false);
    }
}
