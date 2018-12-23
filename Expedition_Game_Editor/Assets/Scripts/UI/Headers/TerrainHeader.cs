using UnityEngine;
using System.Collections;

public class TerrainHeader : MonoBehaviour, IHeader
{
    private SubController subController;

    public SelectionElement editor_button;

    public void Activate(SubController new_subController)
    {
        subController = new_subController;

        SetEditorButton();

        gameObject.SetActive(true);
    }

    private void SetEditorButton()
    {
        //Set selection type to match the list type from which the terrain controller was opened,
        //so that the editor opens a different path
        editor_button.data.type = subController.controller.path.route[subController.controller.step - 3].origin.listManager.listData.data.type;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
