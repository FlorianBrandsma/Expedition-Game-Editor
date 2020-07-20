using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedAction : MonoBehaviour, IAction
{
    private ExInputNumber inputNumber;

    public ActionProperties actionProperties;

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        inputNumber = ActionManager.instance.AddInputNumber(actionProperties);

        inputNumber.enableLimit = true;

        inputNumber.valueType.text = "*";

        inputNumber.min = 0;
        inputNumber.max = 10;

        inputNumber.inputField.characterLimit = 3;
        inputNumber.inputField.contentType = InputField.ContentType.DecimalNumber;

        inputNumber.Value = TimeManager.instance.TimeScale;

        inputNumber.minusButton.onClick.AddListener(delegate { ChangeValue(); });
        inputNumber.plusButton.onClick.AddListener(delegate { ChangeValue(); });

        inputNumber.inputField.onEndEdit.AddListener(delegate { ChangeValue(); });
    }

    public void ChangeValue()
    {
        TimeManager.instance.TimeScale = inputNumber.Value;
    }

    public void UpdateAction()
    {
        if (inputNumber == null) return;

        inputNumber.inputField.text = TimeManager.instance.TimeScale.ToString();
    }

    public void CloseAction() { }
}
