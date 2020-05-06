using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameDisplayAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        //GameDisplayManager.activeDisplay = (GameDisplayManager.Display)path.FindLastRoute(Enums.DataType.GameSave).controller;

        var dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = Enum.GetName(typeof(GameDisplayManager.Display), GameDisplayManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(GameDisplayManager.Display)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.Dropdown.value = (int)GameDisplayManager.activeDisplay;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { GameDisplayManager.SetDisplay(dropdown.Dropdown.value, pathController.route); });
    }

    public void CloseAction() { }
}
