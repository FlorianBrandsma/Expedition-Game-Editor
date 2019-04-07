using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class ElementHeader : MonoBehaviour, IHeader
{
    private EditorController controller;

    public IndexSwitch index_switch;

    public SelectionElement main_element;
    public RawImage icon;
    public InputField input_field;
    public Text id;

    public void Activate(EditorController new_controller)
    {
        controller = new_controller;

        SetHeader();

        gameObject.SetActive(true);
    }

    public void UpdateHeader()
    {

    }

    private void SetHeader()
    {
        //input_field.text = itemController.itemData.name;

        if (index_switch != null)
        {
            //int index_limit = controller.pathController.route.origin.selectionElement.listManager.listProperties.dataController.data_list.Cast<IList>().ToList().Count;
            index_switch.Activate();
        }
    }

    public void Deactivate()
    {
        if (index_switch != null)
            index_switch.Deactivate();

        gameObject.SetActive(false);
    }
}
