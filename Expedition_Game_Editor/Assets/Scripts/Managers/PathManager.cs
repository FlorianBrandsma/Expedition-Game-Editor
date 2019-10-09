using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathManager
{
    #region Paths

    #region Initializers

    #region Main

    public class Main
    {
        EditorForm form = EditorManager.editorManager.forms[0];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(CreateRoutes(source, form, Enums.SelectionGroup.Main), form);
        }
    }

    #endregion

     #region Form (Temporary)

    public class Form
    {
        EditorForm form;

        List<int> source = new List<int>() { 0 };

        public Form(EditorForm new_form)
        {
            form = new_form;
        }

        public Path Initialize()
        {
            return CreatePath(CreateRoutes(source, form, Enums.SelectionGroup.Main), form);
        }
    }

    #endregion

    #endregion

    #region Editor

    #region Structure
    public class Structure
    {
        Path path;
        Route route;

        int enter   = 0;
        int edit    = 1;

        EditorForm form  = EditorManager.editorManager.forms[0];

        public Structure(SelectionElement selection, Route route) //Combine existing path with new route
        {
            this.route = route;

            path = selection.DisplayManager.Display.DataController.SegmentController.Path;
        }

        public Path Enter()
        {
            route.controller = enter;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }

        public Path Edit()
        {
            route.controller = edit;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
        }
    }
    #endregion

    #region Item

    public class Item
    {
        SelectionElement selection;

        Route route;

        List<int> enter;
        List<int> edit;
        List<int> get;

        public Item(SelectionElement selection, Route route)
        {
            this.selection = selection;

            this.route = route;

            var itemDataElement = (ItemDataElement)selection.data.dataElement;

            enter   = new List<int>() { 0, 0, 0, itemDataElement.Type };
            edit    = new List<int>() { 0, 1, 0, itemDataElement.Type };
            get     = new List<int>() { 1 };
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[1];
            return CreatePath(CreateRoutes(enter, route, selection.selectionGroup), form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, selection.selectionGroup), form);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route, selection.selectionGroup), form);
        }
    }

    #endregion

    #region Interactable

    public class Interactable
    {
        SelectionElement selection;

        Path path;
        Route route;

        int enter;
        List<int> edit;

        public Interactable(SelectionElement selection, Route route)
        {
            this.selection = selection;

            this.route = route;

            enter   = 0;
            edit    = new List<int>() { 0, 2, 0, 0 };
        }

        public Path Enter()
        {
            route.controller = enter;
            //Looks convoluted
            path = selection.DisplayManager.Display.DataController.SegmentController.Path;

            EditorForm form = EditorManager.editorManager.forms[0];
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, selection.selectionGroup), form);
        }

        public Path Get()
        {
            return null;
        }
    }

    #endregion

    #region ObjectGraphic

    public class ObjectGraphic
    {
        SelectionElement selection;

        Route route;

        List<int> get;

        public ObjectGraphic(SelectionElement selection, Route route)
        {
            this.selection = selection;

            this.route = route;

            get = new List<int>() { 1 };
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route, selection.selectionGroup), form);
        }
    }

    #endregion

    #region Region

    public class Region
    {
        SelectionElement selection;
        Path path;
        Route route;

        RegionDataElement regionDataElement;

        List<int> enter = new List<int>() { 0, 3 };
        List<int> edit  = new List<int>() { 0, 4 };

        EditorForm form = EditorManager.editorManager.forms[0];

        public Region(SelectionElement selection, Route route)
        {
            this.selection = selection;

            this.route = route;

            regionDataElement = (RegionDataElement)route.data.dataElement;

            if (selection.DisplayManager != null)
                path = selection.DisplayManager.Display.DataController.SegmentController.Path;
        }

        public Path Enter()
        {
            //CreatePath OR ExtendPath based on route.data.type
            //Phase and Interaction extends current path
            //Base stands alone
            List<Route> routes;

            //Reset display to tiles, only when editor is manually opened
            //RegionDisplayManager.ResetDisplay();
            switch (regionDataElement.type)
            {
                case Enums.RegionType.Base:
                    path = CreatePath(CreateRoutes(enter, route, selection.selectionGroup), form);
                    break;
                case Enums.RegionType.Phase:
                    routes = CreateRoutes(enter, route, selection.selectionGroup);
                    path = ExtendPath(path, routes);
                    break;
            }

            path.type = Path.Type.New;

            return path;
        }

        public Path Edit()
        {
            return CreatePath(CreateRoutes(edit, route, selection.selectionGroup), form);
        }

        public Path Open()
        {
            List<int> open = new List<int>() { 1, (int)regionDataElement.type };
            
            Route customRoute = new Route(1, route.data, selection.selectionGroup);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, selection.selectionGroup));
            path.type = Path.Type.New;

            return path;
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        int edit = 0;

        SelectionElement selection;
        Path path;
        Route route;

        EditorForm form = EditorManager.editorManager.forms[0];

        public Terrain(SelectionElement selection, Route route)
        {
            this.selection = selection;
            this.route = route;
        }

        public Path Edit()
        {
            route.controller = edit;

            path = selection.DisplayManager.Display.DataController.SegmentController.Path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }
    }

    public class SceneInteractable
    {
        SelectionElement selection;
        Path path;
        Route route;
        EditorForm form = EditorManager.editorManager.forms[0];

        int enter = 0;

        public SceneInteractable(SelectionElement selection, Route route)
        {
            this.selection = selection;

            this.route = route;
        }

        public Path Enter()
        {
            route.controller = enter;

            path = selection.DisplayManager.Display.DataController.SegmentController.Path;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
        }

        public Path Open()
        {
            List<int> source = new List<int>() { 0, 5 };

            route.controller = enter;

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionGroup.Main);
            return ExtendPath(form.activePath, routes);
        }
    }

    public class SceneObject
    {
        Path path;
        Route route;
        EditorForm form = EditorManager.editorManager.forms[0];

        public SceneObject(SelectionElement selection, Route route)
        {
            this.route = route;
        }

        public Path Enter()
        {
            List<int> source = new List<int>() { 1 };

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionGroup.Main);

            return new Path(form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes), form, form.activePath.start);
        }
    }

    #endregion

    #region Options

    public class Option
    {
        SelectionElement selection;
        Route route;

        List<int> enter = new List<int>() { 0 };

        public Option(SelectionElement selection, Route route)
        {
            this.selection = selection;

            this.route = route;
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(enter, route, selection.selectionGroup), form);
        }
    }

    #endregion

    #region Search

    public class Search
    {
        SelectionElement selection;
        Route route;

        List<int> controllers = new List<int>() { 1 };

        public Search(SelectionElement selection, Route route)
        {
            this.selection = selection;
            this.route = route;

            route.data = new Route.Data(route.data.dataController, route.data.dataElement, selection.dataController.SearchParameters);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(controllers, route, selection.selectionGroup), form);
        }
    }

    #endregion

    #endregion

    #endregion

    #region Methods

    static public List<Route> CreateRoutes(List<int> controllers, EditorForm form, Enums.SelectionGroup selectionGroup)
    {
        return CreateRoutes(controllers, new Route(form.activePath), selectionGroup);
    }

    static public List<Route> CreateRoutes(List<int> controllers, Route route, Enums.SelectionGroup selectionGroup)
    {
        List<Route> routes = new List<Route>();

        foreach(int controller in controllers)
            routes.Add(new Route(controller, route.data, selectionGroup));

        return routes;
    }

    static public Path CreatePath(List<Route> routes, EditorForm form)
    {
        Path path   = new Path();
        path.form   = form;

        foreach (Route route in routes)
            path.Add(new Route(route));

        return path;
    }

    static public Path ExtendPath(Path head, List<Route> tail)
    {
        Path path = new Path(head.CombineRoute(tail), head.form, head.route.Count);

        path.type = head.type;

        return path;
    }

    static public Path ReloadPath(Path path, Route new_route)
    {
        Path newPath = new Path(true);

        newPath.form = path.form;

        foreach (Route route in path.route)
            newPath.Add(route);

        newPath.route[newPath.route.Count - 1] = new_route;

        newPath.start = path.start;

        newPath.type = Path.Type.Reload;

        return newPath;
    }

    //There might be a problem where the original path, for checking loading, is changed
    //together with the new (temp) path
    static public Path ReloadPath(Path path, Route.Data data)
    {
        Path tempPath = new Path(true);

        tempPath.form = path.form;

        foreach (Route route in path.route)
            tempPath.Add(route);

        tempPath.route.Last().data = data;

        tempPath.start = path.start;

        tempPath.type = Path.Type.Reload;

        return tempPath;
    }

    static public Path ReloadPath(Path path, Route.Data data, int step)
    {
        Path newPath = new Path(true);

        newPath.form = path.form;

        foreach (Route route in path.route)
            newPath.Add(route);

        newPath.route[step].data = data;

        newPath.start = path.start;

        newPath.type = Path.Type.Reload;

        return newPath;
    }

    #endregion
}