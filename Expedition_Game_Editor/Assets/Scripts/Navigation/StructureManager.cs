using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StructureManager : MonoBehaviour
{
    private EditorController editorController;

    List<int> id_list = new List<int>();

    void SetRows()
    {
        id_list.Clear();

        for (int i = 0; i < 15; i++)
        {
            //Example:
            //Phase Menu
            //Selected: <Quest> 1
            //Display: All <Phase> where <Quest>_id = 1

            //SELECT id FROM (TABLE) WHERE ID = (ID) AND INDEX = (i)
            id_list.Add(i + 1);
        }
    }

    public void SetStructure(EditorController new_editorController, Path path, int depth, string table, int id)
    {
        editorController = new_editorController;

        SetRows();

        Dropdown structure_dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        structure_dropdown.options.Clear();

        structure_dropdown.onValueChanged.RemoveAllListeners();

        //Separate for-loops to assign value inbetween without triggering "OnValueChange" (endless loop)
        for (int i = 0; i < id_list.Count; i++)
        {
            structure_dropdown.options.Add(new Dropdown.OptionData(table + " " + id_list[i]));
        }

        structure_dropdown.captionText.text = table + " " + id;

        structure_dropdown.value = id_list.IndexOf(id);

        structure_dropdown.onValueChanged.AddListener(delegate { OpenStructure(NewPath(GetComponent<EditorController>().controller_path, depth, structure_dropdown)); });

        //Open Editor
    }

    public void OpenStructure(Path new_path)
    {
        editorController.editorField.windowManager.OpenPath(new_path);
    }

    public string PathString(Path path)
    {
        string path_string = "editor: ";

        for(int i = 0; i < path.editor.Count; i++)
        {
            path_string += path.editor[i] + ",";
        }

        path_string += "id: ";

        for(int i = 0; i < path.id.Count; i++)
        {
            path_string += path.id[i] + ",";
        }

        return path_string;
    }

    public Path NewPath(Path path, int depth, Dropdown dropdown)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < depth; i++)
            new_path.editor.Add(path.editor[i]);

        for (int i = 0; i < depth; i++)
            new_path.id.Add(path.id[i]);

        int id = id_list[dropdown.value];

        new_path.id[new_path.id.Count - 1] = id;

        return new_path;
    }
}
