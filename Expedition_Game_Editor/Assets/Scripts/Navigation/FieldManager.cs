using UnityEngine;
using System.Collections;

public class FieldManager : MonoBehaviour, IEditor
{
    public GameObject content;

    public void OpenEditor()
    {
        gameObject.SetActive(true);
    }

    public void SaveEdit()
    {
        //GetComponent<IEditor>().SaveEdit();
    }

    public void ApplyEdit()
    {
        //GetComponent<IEditor>().ApplyEdit();
    }

    public void CancelEdit()
    {
        NavigationManager.navigation_manager.PreviousEditor();
    }

    public void CloseEditor()
    {
        gameObject.SetActive(false);
    }
}
