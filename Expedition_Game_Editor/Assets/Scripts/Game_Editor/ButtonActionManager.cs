using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonActionManager : MonoBehaviour
{
    public Button   save_button,
                    apply_button,
                    cancel_button;

    public Toggle   trash_toggle;

    public void SetButtons(SubEditor editor)
    {
        if (save_button != null)
            save_button.onClick.AddListener(delegate { editor.SaveEdit(); });

        if (apply_button != null)
            apply_button.onClick.AddListener(delegate { editor.ApplyEdit(); });

        cancel_button.onClick.AddListener(delegate { editor.CancelEdit(); });
    }

    public void CloseButtons()
    {
        if (save_button != null)
            save_button.onClick.RemoveAllListeners();

        if(apply_button != null)
            apply_button.onClick.RemoveAllListeners();

        cancel_button.onClick.RemoveAllListeners();

        trash_toggle.isOn = false;
    }
}
