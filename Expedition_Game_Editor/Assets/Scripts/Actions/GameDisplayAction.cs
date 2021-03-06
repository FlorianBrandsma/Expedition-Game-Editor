﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameDisplayAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;
    private ExDropdown dropdown;

    private Path basePath;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        int index = 0;
        
        if (PathController.route.path.type == Path.Type.New)
        {
            basePath = path.Clone();
            basePath.type = Path.Type.Loaded;

            GameDisplayManager.activeDisplay = GameDisplayManager.Display.Game;
        }

        index = (int)GameDisplayManager.activeDisplay;
        
        if (path.routeList.Count < (PathController.route.path.routeList.Count + 1))
        {
            path.Add(index);
            path.Add(0);
        }
    }

    public void SetAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = Enum.GetName(typeof(GameDisplayManager.Display), GameDisplayManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(GameDisplayManager.Display)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.Dropdown.value = (int)GameDisplayManager.activeDisplay;
        
        dropdown.Dropdown.onValueChanged.AddListener(delegate { GameDisplayManager.SetDisplay(dropdown.Dropdown.value, basePath); });
    }

    public void CloseAction()
    {
        if(dropdown != null)
            dropdown.Dropdown.onValueChanged.RemoveAllListeners();
    }
}
