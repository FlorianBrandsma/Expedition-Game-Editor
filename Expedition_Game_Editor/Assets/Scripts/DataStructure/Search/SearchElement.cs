using UnityEngine;
using System.Collections.Generic;

public class SearchElement : MonoBehaviour
{
    public Route route = new Route();

    public SegmentController segmentController;

    public GameObject glow;

    public void SelectElement()
    {
        List<int> controllers = new List<int>() { 1 };

        SearchData searchData = new SearchData();

        Data data = new Data(new[] { searchData });

        route.data = data;

        Path path = PathManager.CreatePath(PathManager.CreateRoutes(controllers, route), EditorManager.editorManager.forms[2], null);

        //Open path directly to the Search panel
        //Form MUST open

        EditorManager.editorManager.InitializePath(path);
        EditorManager.editorManager.forms[2].formComponent.OpenFormExternally();

        //EditorPath editorPath = new EditorPath(this);

        //switch (route.property)
        //{
        //    case SelectionManager.Property.Get:
        //        EditorManager.editorManager.InitializePath(editorPath.path);
        //        ActivateSelection();
        //        break;
        //    default:
        //        break;
        //}

    }


    //Path path;
    //Route route;
    //SelectionElement origin;

    //List<int> enter = new List<int>() { 0, 3 };
    //List<int> edit = new List<int>() { 0, 4 };

    //EditorForm form = EditorManager.editorManager.forms[0];

    //public Region(Route route, SelectionElement origin)
    //{
    //    this.route = route;
    //    this.origin = origin;

    //    if (origin.listManager != null)
    //        path = origin.listManager.listProperties.segmentController.path;
    //}

    //public Path Enter()
    //{
    //    //CreatePath OR ExtendPath based on route.data.type
    //    //Phase and Task extends current path
    //    //Base stands alone
    //    List<Route> routes;

    //    //Reset display to tiles, only when editor is manually opened
    //    RegionDisplayManager.ResetDisplay();

    //    switch (route.GeneralData().type)
    //    {
    //        case (int)Enums.RegionType.Base:
    //            path = CreatePath(CreateRoutes(enter, route), form, origin);
    //            break;
    //        case (int)Enums.RegionType.Phase:
    //            routes = CreateRoutes(enter, route);
    //            path = ExtendPath(path, routes, origin);
    //            break;
    //            //case (int)Enums.RegionType.Task:
    //            //    routes = CreateRoutes(enter, route);
    //            //    path = ExtendPath(path, routes, origin);
    //            //    break;
    //    }

    //    path.type = Path.Type.New;

    //    return path;
    //}

    //public Path Edit()
    //{
    //    return CreatePath(CreateRoutes(edit, route), form, origin);
    //}
}
