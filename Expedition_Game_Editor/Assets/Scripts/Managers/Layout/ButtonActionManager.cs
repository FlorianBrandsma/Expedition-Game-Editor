using UnityEngine;

public class ButtonActionManager : MonoBehaviour
{
    public Enums.ButtonActionType applyButtonType, 
                                  closeButtonType;

    public ExButton applyButton,
                    closeButton;

    public ExToggle trashToggle;

    public LayoutSection section;

    public void ClickApply()
    {
        if (applyButtonType == Enums.ButtonActionType.Apply)
            section.ApplyChanges();

        if (applyButtonType == Enums.ButtonActionType.Confirm)
            DialogManager.instance.confirmRequest = true;
    }

    public void ClickToggle()
    {
        section.ToggleRemoval(trashToggle.Toggle.isOn);
    }

    public void ClickClose()
    {
        if (closeButtonType == Enums.ButtonActionType.Close)
            section.CloseEditor();

        if (closeButtonType == Enums.ButtonActionType.Cancel)
            DialogManager.instance.confirmRequest = false;
    }

    public void SetButtons(Enums.ExecuteType executionType, IEditor dataEditor)
    {
        if (applyButton != null)
        {
            switch(executionType)
            {
                case Enums.ExecuteType.Add:
                    applyButton.Button.interactable = dataEditor.Addable();
                    applyButton.label.text = "Add";
                    break;

                case Enums.ExecuteType.Update:
                    applyButton.Button.interactable = dataEditor.Applicable();
                    applyButton.label.text = "Apply";
                    break;

                case Enums.ExecuteType.Remove:
                    applyButton.Button.interactable = dataEditor.Removable();
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
