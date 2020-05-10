using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameDisplayAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        int index = 0;
        
        if (PathController.route.path.type == Path.Type.New)
            GameDisplayManager.activeDisplay = GameDisplayManager.Display.Game;

        index = (int)GameDisplayManager.activeDisplay;
        
        if (path.route.Count < (PathController.route.path.route.Count + 1))
        {
            path.Add(index);
            path.Add(0);
        }
    }

    public void SetAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        var dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = Enum.GetName(typeof(GameDisplayManager.Display), GameDisplayManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(GameDisplayManager.Display)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.Dropdown.value = (int)GameDisplayManager.activeDisplay;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { GameDisplayManager.SetDisplay(dropdown.Dropdown.value, PathController.route.path); });
    }

    public void CloseAction() { }
}
