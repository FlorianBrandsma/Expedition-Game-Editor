using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//ROW MANAGER
//Includes all subeditor specific information
//Information includes: which row elements will be shown (based on table and id)
//What happens when you click on a row element
//Element size
//Element placement (zigzag)

public class RowManager : MonoBehaviour, IEditor
{
    List<int> id_list = new List<int>();

    public int sort_type;
    public int row_size = 50;

    public string table;
    //private int id;
    //item type per tab

    public List<int> select_path;
    public List<int> edit_path;

    public bool zigzag;

    public bool get_select, set_select;

    public void OpenEditor()
    {
        id_list.Clear();

        string selected_table = GetComponent<SubEditor>().table;
        int selected_id = GetComponent<SubEditor>().id;

        for (int i = 0; i < 15; i++)
        {
            //Example:
            //Phase Menu
            //Selected: <Quest> 1
            //Display: All <Phase> where <Quest>_id = 1

            //SELECT id FROM (TABLE) WHERE ID = (ID) AND INDEX = (i)
            id_list.Add(i+1);
        }

        //Probably wrong id
        Path new_select_path    = new Path(select_path, new List<int> { selected_id });
        Path new_edit_path      = new Path(edit_path,   new List<int> { selected_id });

        ListManager listManager = GetComponent<ListOptions>().option_list.GetComponent<ListManager>();

        //Pass all possible information to the List
        listManager.SetupList(sort_type, table, id_list, row_size, new_select_path, new_edit_path, zigzag, get_select, set_select);
    
        gameObject.SetActive(true);
    }

    public void CloseEditor()
    {
        CloseList();

        gameObject.SetActive(false);
    }

    void CloseList()
    {
        GetComponent<ListOptions>().option_list.GetComponent<ListManager>().CloseList();
    }
}
