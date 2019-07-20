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
            return CreatePath(CreateRoutes(source, form, Enums.SelectionGroup.Main), form, null);
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
            return CreatePath(CreateRoutes(source, form, Enums.SelectionGroup.Main), form, null);
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
        ListManager origin;

        int enter   = 0;
        int edit    = 1;

        EditorForm form  = EditorManager.editorManager.forms[0];

        public Structure(SelectionElement selection) //Combine existing path with new route
        {
            route = selection.route;
            origin = selection.ListManager;

            path = origin.listProperties.DataController.SegmentController.Path;
        }

        public Path Enter()
        {
            route.controller = enter;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, origin, path.start);
        }

        public Path Edit()
        {
            route.controller = edit;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, origin);
        }
    }
    #endregion

    #region Item

    public class Item
    {
        SelectionElement selectionElement;

        Route route;
        ListManager origin;

        List<int> enter;
        List<int> edit;
        List<int> get;

        public Item(SelectionElement selection)
        {
            selectionElement = selection;

            route = selection.route;
            origin = selection.ListManager;

            var itemDataElement = (ItemDataElement)route.data.DataElement;

            enter   = new List<int>() { 0, 0, 0, itemDataElement.Type };
            edit    = new List<int>() { 0, 1, 0, itemDataElement.Type };
            get     = new List<int>() { 1 };
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[1];
            return CreatePath(CreateRoutes(enter, route, selectionElement.selectionGroup), form, origin);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, selectionElement.selectionGroup), form, origin);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route, selectionElement.selectionGroup), form, origin);
        }
    }

    #endregion

    #region Interactable

    public class Interactable
    {
        SelectionElement selectionElement;

        Path path;
        Route route;
        ListManager origin;

        int enter;
        List<int> edit;

        public Interactable(SelectionElement selection)
        {
            selectionElement = selection;

            route = selection.route;
            origin = selection.ListManager;

            enter   = 0;
            edit    = new List<int>() { 0, 2, 0, 0 };
        }

        public Path Enter()
        {
            route.controller = enter;
            //Looks convoluted
            path = origin.listProperties.DataController.SegmentController.Path;

            EditorForm form = EditorManager.editorManager.forms[0];
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, origin);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, selectionElement.selectionGroup), form, origin);
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
        SelectionElement selectionElement;

        Route route;
        ListManager origin;

        List<int> get;

        public ObjectGraphic(SelectionElement selection)
        {
            selectionElement = selection;

            route = selection.route;
            origin = selection.ListManager;

            get = new List<int>() { 1 };
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route, selectionElement.selectionGroup), form, origin);
        }
    }

    #endregion

    #region Region

    public class Region
    {
        SelectionElement selectionElement;
        Path path;
        Route route;
        ListManager origin;
        RegionDataElement regionDataElement;

        List<int> enter = new List<int>() { 0, 3 };
        List<int> edit  = new List<int>() { 0, 4 };

        EditorForm form = EditorManager.editorManager.forms[0];

        public Region(SelectionElement selection)
        {
            selectionElement = selection;

            route = selection.route;
            origin = selection.ListManager;

            regionDataElement = (RegionDataElement)route.data.DataElement;

            if (selection.ListManager != null)
                path = selection.ListManager.listProperties.DataController.SegmentController.Path;
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
                    path = CreatePath(CreateRoutes(enter, route, selectionElement.selectionGroup), form, origin);
                    break;
                case Enums.RegionType.Phase:
                    routes = CreateRoutes(enter, route, selectionElement.selectionGroup);
                    path = ExtendPath(path, routes, origin);
                    break;
            }

            path.type = Path.Type.New;

            return path;
        }

        public Path Edit()
        {
            return CreatePath(CreateRoutes(edit, route, selectionElement.selectionGroup), form, origin);
        }

        public Path Open()
        {
            List<int> open = new List<int>() { 1, (int)regionDataElement.type };
            
            Route customRoute = new Route(1, route.data, selectionElement.selectionGroup);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, selectionElement.selectionGroup), origin);
            path.type = Path.Type.New;

            return path;
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        int edit = 0;

        Path path;
        Route route;
        ListManager origin;

        EditorForm form = EditorManager.editorManager.forms[0];

        public Terrain(SelectionElement selection)
        {
            route = selection.route;
            origin = selection.ListManager;
        }

        public Path Edit()
        {
            route.controller = edit;

            path = origin.listProperties.DataController.SegmentController.Path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, origin, path.start);
        }
    }

    public class TerrainInteractable
    {
        SelectionElement selectionElement;
        Path path;
        Route route;
        ListManager origin;
        EditorForm form = EditorManager.editorManager.forms[0];

        int enter = 0;

        public TerrainInteractable(SelectionElement selection)
        {
            selectionElement = selection;
            route = selection.route;
            origin = selection.ListManager;
        }

        public Path Enter()
        {
            route.controller = enter;

            path = origin.listProperties.DataController.SegmentController.Path;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, origin);
        }

        public Path Open()
        {
            List<int> source = new List<int>() { 0, 5 };

            route.controller = enter;

            List<Route> routes = CreateRoutes(source, selectionElement.route, Enums.SelectionGroup.Main);
            return ExtendPath(form.activePath, routes, origin);

            //return CreatePath(CreateRoutes(source, selectionElement.route, Enums.SelectionGroup.Main), form, origin);
        }
    }

    public class TerrainObject
    {
        SelectionElement selectionElement;
        Path path;
        Route route;
        ListManager origin;
        EditorForm form = EditorManager.editorManager.forms[0];

        public TerrainObject(SelectionElement selection)
        {
            selectionElement = selection;
            origin = selection.ListManager;
        }

        public Path Enter()
        {
            List<int> source = new List<int>() { 1 };

            List<Route> routes = CreateRoutes(source, selectionElement.route, Enums.SelectionGroup.Main);
            
            return new Path(form.activePath.CombineRoute(routes), form, origin, form.activePath.start);
        }
    }

    #endregion

    #region Options

    public class Option
    {
        SelectionElement selectionElement;
        Route route;
        ListManager origin;

        List<int> enter = new List<int>() { 0 };

        public Option(SelectionElement selection)
        {
            selectionElement = selection;

            route = selection.route;
            origin = selection.ListManager;
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(enter, route, selectionElement.selectionGroup), form, origin);
        }
    }

    #endregion

    #region Search

    public class Search
    {
        SelectionElement selectionElement;
        Route route;

        List<int> controllers = new List<int>() { 1 };

        public Search(SelectionElement selection)
        {
            selectionElement = selection;
            route = selection.route;
            
            route.data = new Data(route.data.DataController, route.data.DataElement, selection.dataController.SearchParameters);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(controllers, route, selectionElement.selectionGroup), form, null);
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

    static public Path CreatePath(List<Route> routes, EditorForm form, ListManager origin)
    {
        Path path   = new Path();
        path.form   = form;
        path.origin = origin;

        foreach (Route route in routes)
            path.Add(route);

        return path;
    }

    static public Path ExtendPath(Path head, List<Route> tail, ListManager origin)
    {
        Path path = new Path(head.CombineRoute(tail), head.form, origin, head.route.Count);

        path.type = head.type;

        return path;
    }

    static public Path ReloadPath(Path new_path, Route new_route)
    {
        Path path = new Path(true);

        path.form = new_path.form;

        foreach (Route route in new_path.route)
            path.Add(route);

        path.route[path.route.Count - 1] = new_route;

        path.start = new_path.start;

        path.type = Path.Type.Reload;

        return path;
    }

    static public Path ReloadPath(Path path, Data data)
    {
        Path temppath = new Path(true);

        temppath.form = path.form;

        foreach (Route route in path.route)
            temppath.Add(route);

        temppath.route.Last().data = data;

        temppath.start = path.start;

        temppath.type = Path.Type.Reload;

        return temppath;
    }

    static public Path ReloadPath(Path path, Data data, int step)
    {
        Path new_path = new Path(true);

        new_path.form = path.form;

        foreach (Route route in path.route)
            new_path.Add(route);

        new_path.route[step].data = data;

        new_path.start = path.start;

        new_path.type = Path.Type.Reload;

        return new_path;
    }

    #endregion
}