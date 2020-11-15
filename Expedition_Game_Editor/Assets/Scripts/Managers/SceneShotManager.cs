using UnityEngine;
using System.Linq;

static public class SceneShotManager
{
    static public Enums.SceneShotType activeShotType;

    static public SceneShotElementData GetActiveElementData(Route route)
    {
        var sceneShotElementData = route.data.dataList.Cast<SceneShotElementData>().Where(x => x.Type == (int)activeShotType).First();

        return sceneShotElementData;
    }

    static public string ShotDescription(Enums.SceneShotType type)
    {
        var description = "Shot " + System.Enum.GetName(typeof(Enums.SceneShotType), type).ToLower();

        return description;
    }

    static public void AddShotRoute(Path path)
    {
        //Adds the scene shot route to the path of a scene region if it hasn't already been added and the scene shot type is not "base"
        //Only add the scene shot route when the last route is region so it won't try to add it when opening actors or props
        if (RegionManager.regionType == Enums.RegionType.Scene && path.routeList.Last().data.dataController.DataType == Enums.DataType.Region)
        {
            var sceneShotRouteSource = path.FindLastRoute(Enums.DataType.SceneShot);
            var sceneShotElementData = GetActiveElementData(sceneShotRouteSource);

            if ((Enums.SceneShotType)sceneShotElementData.Type != Enums.SceneShotType.Base)
            {
                var sceneShotRoute = new Route()
                {
                    controllerIndex = 3,
                    id = sceneShotElementData.Id,
                    data = sceneShotRouteSource.data,
                    path = path,

                    selectionStatus = sceneShotRouteSource.selectionStatus
                };

                path.Add(sceneShotRoute);
            }
        }
    }

    static public void SetShot(Enums.SceneShotType shot, Path path)
    {
        activeShotType = shot;
        
        if (path.routeList.Last().data.dataController.DataType == Enums.DataType.SceneShot)
        {
            path = path.TrimToLastType(Enums.DataType.Region);   
        }

        RenderManager.Render(path);
    }
}
