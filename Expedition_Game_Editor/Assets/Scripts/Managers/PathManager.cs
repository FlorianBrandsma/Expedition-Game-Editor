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
            return CreatePath(CreateRoutes(source, form), form);
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
            return CreatePath(CreateRoutes(source, form), form);
        }
    }

    #endregion

    #endregion

    #region Editor

    #region Structure
    public class Structure
    {
        Route route;

        Path path;

        int enter   = 0;
        int edit    = 1;

        EditorForm form  = EditorManager.editorManager.forms[0];

        public Structure(Route new_route) //Combine existing path with new route
        {
            route = new_route;

            path = route.origin.selectionElement.listManager.listProperties.dataController.segmentController.path;
        }

        public Path Enter()
        {
            route.controller = enter;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
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
        List<int> enter;
        List<int> edit;
        List<int> get;

        Route route;

        public Item(Route new_route)
        {
            route = new_route;

            enter   = new List<int>() { 0, 0, 0, route.GeneralData().type };
            edit    = new List<int>() { 0, 1, 0, route.GeneralData().type };
            get     = new List<int>() { 1 };
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[1];
            return CreatePath(CreateRoutes(enter, route), form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route), form);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route), form);
        }
    }

    #endregion

    #region Element

    public class Element
    {
        int enter;
        List<int> edit;

        Route route;
        Path path;

        public Element(Route new_route)
        {
            route   = new_route;

            enter   = 0;
            edit    = new List<int>() { 0, 2, 0, new_route.GeneralData().type };
        }

        public Path Enter()
        {
            route.controller = enter;

            path = route.origin.selectionElement.listManager.listProperties.dataController.segmentController.path;

            EditorForm form = EditorManager.editorManager.forms[0];
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route), form);
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
        List<int> get;

        Route route;

        public ObjectGraphic(Route new_route)
        {
            route = new_route;

            get = new List<int>() { 1 };
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route), form);
        }
    }

    #endregion

    #region Region

    public class Region
    {
        List<int> enter = new List<int>() { 0, 3 };
        List<int> edit  = new List<int>() { 0, 4 };

        EditorForm form = EditorManager.editorManager.forms[0];
        Route route;
        Path path;

        public Region(Route new_route)
        {
            route = new_route;
        }

        public Path Enter()
        {
            //CreatePath OR ExtendPath based on route.data.type
            //Phase and Task extends current path
            //Base stands alone
            List<Route> routes;

            //Reset display to tiles, only when editor is manually opened
            RegionDisplayManager.ResetDisplay();

            switch (route.GeneralData().type)
            {
                case (int)RegionManager.Type.Base:
                    path = CreatePath(CreateRoutes(enter, route), form);
                    break;
                case (int)RegionManager.Type.Phase:
                    routes = CreateRoutes(enter, route);
                    path = ExtendPath(route.path, routes);
                    break;
                case (int)RegionManager.Type.Task:
                    routes = CreateRoutes(enter, route);
                    path = ExtendPath(route.path, routes);
                    break;
            }

            path.type = Path.Type.New;

            return path;
        }

        public Path Edit()
        {
            return CreatePath(CreateRoutes(edit, route), form);
        }

        public Path Open()
        {
            List<int> open = new List<int>() { 1, route.GeneralData().type };

            Route custom_route = new Route(1, route.data, route.data_type, route.origin);

            path = ExtendPath(route.path, CreateRoutes(open, custom_route));
            path.type = Path.Type.New;

            return path;
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        int edit = 0;
        List<int> enter;

        Route route;
        Path path;

        EditorForm form = EditorManager.editorManager.forms[0];

        public Terrain(Route new_route)
        {
            route = new_route;          
        }

        public Path Edit()
        {
            route.controller = edit;

            path = route.origin.selectionElement.listManager.listProperties.dataController.segmentController.path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }
    }

    public class TerrainItem
    {
        Path path;
        Route route;
        EditorForm form = EditorManager.editorManager.forms[0];

        public TerrainItem(Route new_route)
        {
            route = new Route(new_route.GeneralData().type, new_route.data, new_route.data_type, new_route.origin);

            path = form.active_path.Trim(form.active_path.start + 3);
            
            path.Add(route);
        }

        public Path Enter()
        {
            return path;
        }
    }

    #endregion

    #region Options

    public class Option
    {
        List<int> enter = new List<int>() { 0 };

        Route route;

        public Option(Route new_route)
        {
            route = new_route;
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(enter, route), form);
        }
    }

    #endregion

    #endregion

    #endregion

    #region Methods

    static public List<Route> CreateRoutes(List<int> new_controllers, EditorForm new_form)
    {
        return CreateRoutes(new_controllers, new Route(new_form.active_path));
    }

    static public List<Route> CreateRoutes(List<int> new_controllers, Route new_route)
    {
        List<Route> routes = new List<Route>();

        for (int i = 0; i < new_controllers.Count; i++)
            routes.Add(new Route(new_controllers[i], new_route.data, new_route.data_type, new_route.origin));

        return routes;
    }

    static public Path CreatePath(List<Route> new_routes, EditorForm new_form)
    {
        Path path = new Path();
        path.form = new_form;

        foreach (Route route in new_routes)
            path.Add(route);

        return path;
    }

    static public Path ExtendPath(Path head, List<Route> tail)
    {
        Path path = new Path(head.CombineRoute(tail), head.form, head.route.Count);

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

    static public Path ReloadPath(Path new_path, IEnumerable new_data)
    {
        Path path = new Path(true);

        path.form = new_path.form;

        foreach (Route route in new_path.route)
            path.Add(route);

        path.route.Last().data = new_data;

        path.start = new_path.start;

        path.type = Path.Type.Reload;

        return path;
    }

    static public Path ReloadPath(Path new_path, IEnumerable new_data, int step)
    {
        Path path = new Path(true);

        path.form = new_path.form;

        foreach (Route route in new_path.route)
            path.Add(route);

        path.route[step].data = new_data;

        path.start = new_path.start;

        path.type = Path.Type.Reload;

        return path;
    }

    #endregion
}