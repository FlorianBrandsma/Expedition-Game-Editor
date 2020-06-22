using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonActionManager : MonoBehaviour
{
    public Button   applyButton,
                    closeButton;

    public Toggle   trashToggle;

    public void InitializeButtons(LayoutSection section)
    {
        if (applyButton != null)
            applyButton.onClick.AddListener(delegate { section.ApplyChanges(); });
        
        if(closeButton != null)
            closeButton.onClick.AddListener(delegate { section.CloseEditor(); });    
    }

    public void SetButtons(bool changed)
    {
        if (applyButton != null)
            applyButton.interactable = changed;
    }

    public void CloseButtons()
    {
        if(trashToggle != null)
            trashToggle.isOn = false;
    }
}
