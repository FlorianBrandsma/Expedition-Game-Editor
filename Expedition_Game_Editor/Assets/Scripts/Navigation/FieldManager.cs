using UnityEngine;
using System.Collections;

public class FieldManager : MonoBehaviour, IEditor
{
    public GameObject content;

    public GameObject[] fields;

    public TabManager tabManager;

    public void OpenEditor()
    {
        gameObject.SetActive(true);

        if (tabManager != null)
            SetTabs();
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
        CloseFields();

        if (tabManager != null)
            tabManager.CloseTabs();

        gameObject.SetActive(false);
    }

    void CloseFields()
    {
        foreach (GameObject field in fields)
            field.GetComponent<EditorField>().CloseField();
    }
}
