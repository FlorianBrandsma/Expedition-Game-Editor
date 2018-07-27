using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SelectionElement : MonoBehaviour
{
    //Hide in inspector
    public string table;
    public int id;

    public Enums.SelectionType selectionType;

    public SelectionField selectionField;

    public Text id_text, header, content;

    //PanelElement
    public Button edit_button;

    public ListManager listManager { get; set; }

    public GameObject glow;

    public void InitializeSelection(ListManager new_listManager, int index, Enums.SelectionType new_selectionType)
    {
        listManager = new_listManager;

        table = listManager.table;
        id = listManager.id_list[index];

        selectionType = new_selectionType;
    }

    public void SelectElement()
    {
        selectionField.SelectElement(this);
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
}
