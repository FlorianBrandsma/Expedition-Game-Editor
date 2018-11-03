using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorHeader : MonoBehaviour
{
    public SelectionElement main_element;
    public InputField input_field;
    public Text id;
    public IndexSwitch index_switch;

    public void Activate()
    {
        if(index_switch != null)
        {
            index_switch.Activate();
        }

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        if(index_switch != null)
            index_switch.Deactivate();

        gameObject.SetActive(false);
    }
}
