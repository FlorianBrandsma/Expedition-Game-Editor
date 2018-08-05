using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StructureManager : MonoBehaviour
{
    public void SetStructure(ListData listData)
    {
        Dropdown structure_dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        structure_dropdown.options.Clear();
        structure_dropdown.onValueChanged.RemoveAllListeners();

        for (int i = 0; i < listData.id_list.Count; i++)
        {
            structure_dropdown.options.Add(new Dropdown.OptionData(listData.controller.data.table + " " + i));
        }

        int selected_index = listData.id_list.IndexOf(listData.controller.data.id);

        structure_dropdown.captionText.text = listData.controller.data.table + " " + selected_index;
        structure_dropdown.value = selected_index;

        structure_dropdown.onValueChanged.AddListener(delegate { OpenPath(listData.controller.data.path, listData.id_list[structure_dropdown.value]); });
    }

    public void OpenPath(Path path, int id)
    {
        EditorManager.editorManager.OpenPath(PathManager.ReloadPath(path, id));
    }
}
