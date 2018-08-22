using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StructureManager : MonoBehaviour
{
    public ListData listData;

    public void SetStructure()
    {
        EditorController controller = GetComponent<EditorController>();
        Dropdown dropdown = controller.actionManager.AddDropdown();

        dropdown.options.Clear();
        dropdown.onValueChanged.RemoveAllListeners();

        for (int i = 0; i < listData.list.Count; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(listData.data.table + " " + i));
        }
        
        int selected_index = listData.list.FindIndex(x => x.id == controller.data.id);

        dropdown.captionText.text = listData.list[selected_index].table + " " + selected_index;
        dropdown.value = selected_index;

        dropdown.onValueChanged.AddListener(delegate { OpenPath(controller.path, listData.list[dropdown.value]); });
    }

    public void OpenPath(Path path, ElementData data)
    {
        EditorManager.editorManager.OpenPath(PathManager.ReloadPath(path, data));
    }
}
