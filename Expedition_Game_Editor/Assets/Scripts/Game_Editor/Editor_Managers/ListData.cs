using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(EditorController))]

public class ListData : MonoBehaviour
{
    public ElementData data;

    ElementData controller_data;

    //0: Label
    //1: Buttons
    //2: Grid

    public Enums.SortType sort_type;

    public List<int> id_list { get; set; }
    public int id_count;

    //public bool relative_index;

    public Path select_path { get; set; }
    public Path edit_path   { get; set; }

    public void InitializeRows()
    {
        id_list = new List<int>();

        for (int i = 0; i < id_count; i++)
            id_list.Add(i + 1);

        //Init select
        //Init edit
    }

    public void SetRows()
    {
        controller_data = GetComponent<IController>().data;

        //pathManager = new PathManager(data);

        //In case this is confusing: it's just a test!
        //Elements get assigned a path by themselves

        /*
        EditorPath path = new EditorPath(data);

        Debug.Log(EditorManager.PathString(path.edit));
        */

        /*
        string selected_table = GetComponent<IController>().GetTable();
        int selected_id = GetComponent<IController>().GetID();

        //Let every sub-editor remember it's path, as the path to manipulate
        if (pathManager.adaptive)
        {
            //1. Get current Path
            //2. Get new index
            //3. Add new index
            //4. Add new id

            
            //Must be: path it's currently on (using depth)

            List<int> new_id_path = CombinePath(this_path.id, new List<int> { selected_id });

            if(pathManager.structure.editor.Count > 0)
                select_path = new Path(CombinePath(this_path.editor, pathManager.structure.editor), new_id_path);

            if(pathManager.edit.editor.Count > 0)
                edit_path = new Path(CombinePath(this_path.editor, pathManager.edit.editor), new_id_path);
        } else {
            //1. Clear existing path
            //2. Create new select path using select_index
            //3. Create new edit path using edit_index
            //4. Create new ID (fill with 0s to the length of the new path. replace last index with selected id)
            if (pathManager.structure.editor.Count > 0)
                select_path = CreatePath(pathManager.structure.editor, selected_id);

            if (pathManager.edit.editor.Count > 0)
                edit_path = CreatePath(pathManager.edit.editor, selected_id);
        }
        */

        ListManager listManager = GetComponent<ListProperties>().main_list.GetComponent<ListManager>();

        listManager.InitializeList(this);

        GetComponent<ListProperties>().SetList();
    }

    public void CloseRows()
    {
        GetComponent<ListProperties>().main_list.GetComponent<ListManager>().CloseList();
    }
}
