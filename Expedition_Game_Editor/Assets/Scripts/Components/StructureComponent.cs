using UnityEngine;
using UnityEngine.UI;

public class StructureComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    public ListProperties listProperties;

    public void InitializeComponent(Path new_path)
    {

    }

    public void SetComponent(Path new_path)
    {
        EditorController controller = GetComponent<EditorController>();
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.options.Clear();
        dropdown.onValueChanged.RemoveAllListeners();

        for (int i = 0; i < listProperties.listData.list.Count; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(listProperties.listData.data.table + " " + i));
        }
        
        int selected_index = listProperties.listData.list.FindIndex(x => x.id == controller.route.data.id);

        dropdown.captionText.text = listProperties.listData.list[selected_index].table + " " + selected_index;
        dropdown.value = selected_index;

        Path path = controller.path;

        dropdown.onValueChanged.AddListener(delegate { OpenPath(path, listProperties.listData.list[dropdown.value]); });
    }

    public void OpenPath(Path path, ElementData data)
    {
        EditorManager.editorManager.OpenPath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() { }
}
