using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainItemHeader : MonoBehaviour, IHeader
{
    public Text label;

    public void Activate(EditorController new_controller)
    {
        GeneralData data = new_controller.pathController.route.GeneralData();

        //Definitely a placeholder
        label.text = data.table + " " + (data.id - 1);

        gameObject.SetActive(true);
    }

    public void UpdateHeader()
    {

    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
