using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(ElementData))]

public class SelectionElement : MonoBehaviour
{
    public ElementData data;
    EditorPath editorPath;

    public Enums.SelectionType selectionType;
    public Enums.SelectionProperty selectionProperty;

    public SelectionGroup selectionGroup;

    public Text id_text, header, content;

    //PanelElement
    public Button edit_button;

    public ListManager listManager { get; set; }

    public GameObject glow;

    public void InitializeSelection(ListManager new_listManager, int index)
    {
        listManager = new_listManager;

        data.table = listManager.table;

        data.id = listManager.id_list[index];

        editorPath = new EditorPath(data);

        
        //Debug.Log(EditorManager.PathString(editorPath.edit));

        selectionType = listManager.selectionType;
        selectionProperty = listManager.selectionProperty;

        Debug.Log(listManager.listData.sort_type);
        if(listManager.listData.sort_type == Enums.SortType.Panel)
        {
            Debug.Log("test");
            GetComponent<Button>().onClick.AddListener(delegate { OpenPath(editorPath.select); });
            //element.GetComponent<Button>().onClick.AddListener(delegate { listManager.OpenPath(listManager.NewPath(select_path, id)); });
            //element.edit_button.onClick.AddListener(delegate { listManager.OpenPath(listManager.NewPath(edit_path, id)); });
        } else {
            if (selectionProperty == Enums.SelectionProperty.Get)
                Debug.Log("test");
            else
                Debug.Log("test");
        }
    }

    public void OpenPath(Path new_path)
    {
        EditorManager.editorManager.OpenPath(new_path);
    }

    public void SelectElement()
    {
        if(selectionType != Enums.SelectionType.None)
            selectionGroup.SelectElement(this);
    }

    public void ActivateElement()
    {
        if (listManager != null)
            listManager.ActivateSelection();

        glow.SetActive(true);
    }

    public void DeactivateElement()
    {
        if(listManager != null)
            listManager.DeactivateSelection(true);

        glow.SetActive(false);
    } 

    public void SetElement(SelectionElement new_element)
    {
        data.id = new_element.data.id;
    }
}
