using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainItemHeader : MonoBehaviour, IHeader
{
    public Text label;

    public void Activate(SubController new_subController)
    {
        ElementData data = new_subController.controller.route.data;

        //Definitely a placeholder
        label.text = data.table + " " + (data.id - 1);

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
