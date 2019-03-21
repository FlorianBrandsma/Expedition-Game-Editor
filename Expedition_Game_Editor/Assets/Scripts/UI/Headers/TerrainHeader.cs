using UnityEngine;
using System.Collections;

public class TerrainHeader : MonoBehaviour, IHeader
{
    private EditorController controller;

    public SelectionElement editor_button;

    public void Activate(EditorController new_controller)
    {
        controller = new_controller;

        SetEditorButton();

        gameObject.SetActive(true);
    }

    public void UpdateHeader()
    {

    }

    private void SetEditorButton()
    {
        //Set selection type to match the list type from which the terrain controller was opened,
        //so that the editor opens a different path
        
        editor_button.data = controller.pathController.path.route[controller.pathController.step - 3].data;
        //editor_button.controller = controller.pathController;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
