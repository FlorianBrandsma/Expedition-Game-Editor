using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonActionManager : MonoBehaviour
{
    public ExButton applyButton,
                    closeButton;

    public ExToggle trashToggle;

    public void InitializeButtons(LayoutSection section)
    {
        if (applyButton != null)
            applyButton.Button.onClick.AddListener(delegate { section.ApplyChanges(); });

        if (trashToggle != null)
            trashToggle.Toggle.onValueChanged.AddListener(delegate { section.ToggleRemoval(trashToggle.Toggle.isOn); });

        if (closeButton != null)
            closeButton.Button.onClick.AddListener(delegate { section.CloseEditor(); });    
    }

    public void SetButtons(Enums.ExecuteType executionType, bool changed)
    {
        if (applyButton != null)
        {
            switch(executionType)
            {
                case Enums.ExecuteType.Add:
                    applyButton.Button.interactable = true;
                    applyButton.label.text = "Add";
                    break;

                case Enums.ExecuteType.Update:
                    applyButton.Button.interactable = changed;
                    applyButton.label.text = "Apply";
                    break;

                case Enums.ExecuteType.Remove:
                    applyButton.Button.interactable = true;
                    applyButton.label.text = "Remove";
                    break;
            }
        }
    }

    public void CloseButtons()
    {
        if(trashToggle != null)
            trashToggle.Toggle.isOn = false;
    }
}
