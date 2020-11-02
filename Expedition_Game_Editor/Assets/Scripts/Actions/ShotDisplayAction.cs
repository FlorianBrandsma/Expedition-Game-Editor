using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ShotDisplayAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path)
    {
        //The right approach is probably that when the action changes here, the opened route takes the
        //pathController's route as a base and then opens index 3.
        
        //Actually, this entire action might be reduntant. The shot data needs to come from somewhere and it should
        //probably come from the path in the same way it's done for interaction destination regions (auto select).
        
        //An empty default shot is included in the data list. The shot editor should only be opened if the selected shot type is not default.
        //The region navigation action can handle the adding of the route in those cases.

        //Next task: create and load shot data

        //if (PathController.route.path.type == Path.Type.New)
        //{
        //    var testRoute = new Route()
        //    {
        //        controllerIndex = 3,
        //        id = 0,
        //        data = new Data(),
        //        path = path,

        //        selectionStatus = Enums.SelectionStatus.Main
        //    };

        //    //path.Add(testRoute);

        //    Debug.Log("Add shot route if this is new: " + PathController.route.path.type);

        //    Debug.Log(RenderManager.PathString(path));
        //}
    }

    public void SetAction(Path path)
    {
        //Use type from data instead
        RegionManager.activeShotType = Enums.SceneShotType.Base; //(Enums.SceneShotType)path.FindLastRoute(Enums.DataType.SceneShot).controllerIndex;

        var dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = "Shot " + Enum.GetName(typeof(Enums.SceneShotType), RegionManager.activeShotType).ToLower();

        foreach (var shot in Enum.GetValues(typeof(Enums.SceneShotType)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData("Shot " + shot.ToString().ToLower()));
        }

        dropdown.Dropdown.value = (int)RegionManager.activeShotType;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { RegionManager.SetShot(dropdown.Dropdown.value, PathController.route.path); });

        Debug.Log(RenderManager.PathString(PathController.route.path));
    }

    public void CloseAction() { }
}
