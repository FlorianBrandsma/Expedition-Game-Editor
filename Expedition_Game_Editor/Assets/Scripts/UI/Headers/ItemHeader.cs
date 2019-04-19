using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class ItemHeader : MonoBehaviour, IHeader
{
    private EditorController controller;
    private ItemController itemController;

    public IndexSwitch index_switch;

    public SelectionElement main_element;
    public RawImage icon;
    public InputField input_field;
    public Text id;

    public void Activate(EditorController new_controller)
    {
        controller = new_controller;
        itemController = (ItemController)controller.pathController.dataController;

        SetHeader();

        gameObject.SetActive(true);
    }
    
    public void UpdateHeader()
    {
        SetIcon();
    }

    private void SetHeader()
    {
        //input_field.text = itemController.itemData.name;

        //main_element.data = itemController.objectGraphicData;

        SetIcon();

        if (index_switch != null)
        {
            //int index_limit = subController.controller.route.origin.selectionElement.manager.properties.dataList.list.Count;
            //int index_limit = 15;
            index_switch.Activate();
        }          
    }

    private void SetIcon()
    {
        icon.texture = Resources.Load<Texture2D>("Textures/Objects/Icons/" + main_element.GeneralData().id);
    }

    public void Deactivate()
    {
        if (index_switch != null)
            index_switch.Deactivate();

        gameObject.SetActive(false);
    }
}
