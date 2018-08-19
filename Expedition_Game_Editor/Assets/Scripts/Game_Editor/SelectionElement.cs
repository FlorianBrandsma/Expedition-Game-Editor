using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SelectionElement : MonoBehaviour
{
    public ElementData data;
    EditorPath editorPath;

    public Enums.SelectionType selectionType;
    public Enums.SelectionProperty selectionProperty;

    public SelectionGroup selectionGroup;

    public Text id_text, header, content;

    //PanelElement exclusive
    public Button edit_button;

    public ListManager listManager { get; set; }

    public GameObject glow;

    void SetData(ElementData new_data)
    {
        data = new_data;

        editorPath = new EditorPath(data);
    }

    public void InitializeSelection(ListManager new_listManager, ElementData data)
    {
        listManager = new_listManager;

        SetData(data);

        selectionType = listManager.selectionType;
        selectionProperty = listManager.selectionProperty;

        //To do: "Select" element before opening

        if(selectionType != Enums.SelectionType.None)
        {
            if (listManager.listData.sort_type == Enums.SortType.Panel)
            {
                GetComponent<Button>().onClick.AddListener(delegate { OpenPath(editorPath.source); });
                edit_button.onClick.AddListener(delegate { OpenPath(editorPath.edit); });
            }
            else
            {
                if (selectionProperty == Enums.SelectionProperty.Get)
                    GetComponent<Button>().onClick.AddListener(delegate { OpenPath(editorPath.source); });

                if (selectionProperty == Enums.SelectionProperty.Set)
                    GetComponent<Button>().onClick.AddListener(delegate { OpenPath(editorPath.edit); });
            }
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
        glow.SetActive(true);
    }

    public void DeactivateElement()
    {
        glow.SetActive(false);
    } 

    public void SetElement(SelectionElement new_element)
    {
        data.id = new_element.data.id;
    }
}
