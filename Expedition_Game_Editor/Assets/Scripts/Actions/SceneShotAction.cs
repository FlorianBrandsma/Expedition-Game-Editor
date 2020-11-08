using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneShotAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        SceneShotManager.AddShotRoute(path);
    }

    public void SetAction(Path path)
    {
        var dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = SceneShotManager.ShotDescription(SceneShotManager.activeShotType);

        foreach (var type in Enum.GetValues(typeof(Enums.SceneShotType)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(SceneShotManager.ShotDescription((Enums.SceneShotType)type)));
        }

        dropdown.Dropdown.value = (int)SceneShotManager.activeShotType;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { SceneShotManager.SetShot((Enums.SceneShotType)dropdown.Dropdown.value, path); });
    }

    public void CloseAction() { }
}