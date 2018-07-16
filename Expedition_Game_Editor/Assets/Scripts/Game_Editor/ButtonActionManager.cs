using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonActionManager : MonoBehaviour
{
    public Button   save_button,
                    apply_button,
                    close_button;

    public Toggle   trash_toggle;

    public void SetButtons(EditorController controller)
    {
        if (save_button != null)
            save_button.onClick.AddListener(delegate { controller.SaveEdit(); });

        if (apply_button != null)
            apply_button.onClick.AddListener(delegate { controller.ApplyEdit(); });
        
        if(close_button != null)
            close_button.onClick.AddListener(delegate { controller.CancelEdit(); });
    }

    public void CloseButtons()
    {
        if (save_button != null)
            save_button.onClick.RemoveAllListeners();

        if(apply_button != null)
            apply_button.onClick.RemoveAllListeners();

        if(close_button != null)
            close_button.onClick.RemoveAllListeners();

        if(trash_toggle != null)
            trash_toggle.isOn = false;
    }
}
