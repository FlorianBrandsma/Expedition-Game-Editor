using UnityEngine;
using UnityEngine.UI;

public class DefaultHeader : MonoBehaviour, IHeader
{
    //private SubController subController;

    public IndexSwitch index_switch;

    public SelectionElement main_element;
    public InputField input_field;
    public Text id;

    public void Activate(SubController new_subController)
    {
        //subController = new_subController;

        if (index_switch != null)
            index_switch.Activate();

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        if (index_switch != null)
            index_switch.Deactivate();

        gameObject.SetActive(false);
    }
}
