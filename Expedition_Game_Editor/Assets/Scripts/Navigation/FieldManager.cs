using UnityEngine;
using System.Collections;

public class FieldManager : MonoBehaviour, IEditor
{
    public GameObject content;

    public GameObject[] fields;

    public TabManager tabManager;

    //Move local to field

    public void OpenEditor()
    {
        if(tabManager != null)
            SetTabs();

        gameObject.SetActive(true);
    }

    void SetTabs()
    {
        tabManager.SetFieldTabs(fields);
    }
    
    public void SaveEdit()
    {
        //Probably grab a string from IEditorSegment, to use in query
        //GetComponent<IEditor>().SaveEdit();
    }

    public void ApplyEdit()
    {
        //GetComponent<IEditor>().ApplyEdit();
    }

    public void CancelEdit()
    {
        NavigationManager.navigation_manager.PreviousEditor();
        //GetComponent<IEditor>().CancelEdit();
    }

    public void CloseEditor()
    {
        if (tabManager != null)
            tabManager.CloseTabs();

        gameObject.SetActive(false);
    }
}
