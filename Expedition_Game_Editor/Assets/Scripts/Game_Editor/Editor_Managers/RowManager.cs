using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//REVIEW

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

    public int[] select_index;
    public int[] edit_index;

    public bool zigzag;

    public bool get_select, set_select;

    public void GetRows()
    {
        id_list.Clear();

        for (int i = 0; i < 15; i++)
        {
            id_list.Add(i + 1);
        }
    }

    public void SetupRows(bool main_editor)
    {
        ListManager listManager = GetComponent<ListProperties>().main_list.GetComponent<ListManager>();

        string selected_table = GetComponent<SubEditor>().table;
        int selected_id = GetComponent<SubEditor>().id;

        //Let every sub-editor remember it's path, as the path to manipulate

        Path select_path;
        Path edit_path;

        if (edit_index.Length == 1)
        {
            //1. Get current Path
            //2. Get new index
            //3. Add new index
            //4. Add new id

            Path this_path = GetComponent<SubEditor>().path;

            List<int> new_id_path = CombinePath(this_path.id, new int[] { selected_id }, false);

            select_path = new Path(CombinePath(this_path.editor, select_index, main_editor), new_id_path);
            edit_path = new Path(CombinePath(this_path.editor, edit_index, main_editor), new_id_path);
        }
        else
        {
            //1. Clear existing path
            //2. Create new select path using select_index
            //3. Create new edit path using edit_index
            //4. Create new ID (fill with 0s to the length of the new path. replace last index with selected id)

            select_path = CreatePath(select_index, selected_id);
            edit_path = CreatePath(edit_index, selected_id);
        }

        //Pass all possible information to the List
        listManager.SetupList(sort_type, table, id_list, row_size, select_path, edit_path, (edit_index.Length > 0), zigzag, get_select, set_select);
    }

    public void OpenEditor()
    {
        //gameObject.SetActive(true);
    }

    private Path CreatePath(int[] new_editor, int new_id)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < new_editor.Length; i++)
        {
            new_path.editor.Add(new_editor[i]);

            if(i < new_editor.Length - 1)
                new_path.id.Add(0);
        }

        return new_path;
    }

    private List<int> CombinePath(List<int> path, int[] new_index, bool add)
    {
        List<int> new_path = new List<int>();

        for (int i = 0; i < path.Count; i++)
        {
            new_path.Add(path[i]);
        }

        for(int i = 0; i < new_index.Length; i++)
        {
            if (!add)
                new_path[new_path.Count - (i + 1)] = new_index[i];
            else
                new_path.Add(new_index[i]);
        }

        return new_path;
    }

    public void CloseEditor()
    {
        CloseList();

        gameObject.SetActive(false);
    }

    void CloseList()
    {
        GetComponent<ListProperties>().main_list.GetComponent<ListManager>().CloseList();
    }
}
