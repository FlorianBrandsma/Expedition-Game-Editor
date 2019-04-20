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
            return CreatePath(CreateRoutes(source, form), form, null);
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
            return CreatePath(CreateRoutes(source, form), form, null);
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
        SelectionElement origin;

        int enter   = 0;
        int edit    = 1;

        EditorForm form  = EditorManager.editorManager.forms[0];

        public Structure(Route route, SelectionElement origin) //Combine existing path with new route
        {
            this.route  = route;
            this.origin = origin;
            path        = origin.listManager.listProperties.segmentController.path;
        }

        public Path Enter()
        {
            route.controller = enter;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, origin);
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
        Route route;
        SelectionElement origin;

        List<int> enter;
        List<int> edit;
        List<int> get;

        public Item(Route route, SelectionElement origin)
        {
            this.route = route;
            this.origin = origin;

            enter   = new List<int>() { 0, 0, 0, route.GeneralData().type };
            edit    = new List<int>() { 0, 1, 0, route.GeneralData().type };
            get     = new List<int>() { 1 };
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[1];
            return CreatePath(CreateRoutes(enter, route), form, origin);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route), form, origin);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route), form, origin);
        }
    }

    #endregion

    #region Element

    public class Element
    {
        Path path;
        Route route;
        SelectionElement origin;

        int enter;
        List<int> edit;

        public Element(Route route, SelectionElement origin)
        {
            this.route  = route;
            this.origin = origin;

            enter   = 0;
            edit    = new List<int>() { 0, 2, 0, route.GeneralData().type };
        }

        public Path Enter()
        {
            route.controller = enter;
            //Looks convoluted
            path = origin.listManager.listProperties.segmentController.path;

            EditorForm form = EditorManager.editorManager.forms[0];
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, origin);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(CreateRoutes(edit, route), form, origin);
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
        Route route;
        SelectionElement origin;

        List<int> get;

        public ObjectGraphic(Route route, SelectionElement origin)
        {
            this.route  = route;
            this.origin = origin;

            get = new List<int>() { 1 };
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(get, route), form, origin);
        }
    }

    #endregion

    #region Region

    public class Region
    {
        Path path;
        Route route;
        SelectionElement origin;

        List<int> enter = new List<int>() { 0, 3 };
        List<int> edit  = new List<int>() { 0, 4 };

        EditorForm form = EditorManager.editorManager.forms[0];

        public Region(Route route, SelectionElement origin)
        {
            this.route  = route;
            this.origin = origin;

            if(origin.listManager != null)
                path        = origin.listManager.listProperties.segmentController.path;
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
                case (int)Enums.RegionType.Base:
                    path = CreatePath(CreateRoutes(enter, route), form, origin);
                    break;
                case (int)Enums.RegionType.Phase:
                    routes = CreateRoutes(enter, route);
                    path = ExtendPath(path, routes, origin);
                    break;
                //case (int)Enums.RegionType.Task:
                //    routes = CreateRoutes(enter, route);
                //    path = ExtendPath(path, routes, origin);
                //    break;
            }

            path.type = Path.Type.New;

            return path;
        }

        public Path Edit()
        {
            return CreatePath(CreateRoutes(edit, route), form, origin);
        }

        public Path Open()
        {
            List<int> open = new List<int>() { 1, route.GeneralData().type };

            Route custom_route = new Route(1, route.data, route.property);

            path = ExtendPath(route.path, CreateRoutes(open, custom_route), origin);
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

        Path path;
        Route route;
        SelectionElement origin;

        EditorForm form = EditorManager.editorManager.forms[0];

        public Terrain(Route route, SelectionElement origin)
        {
            this.route  = route;
            this.origin = origin;
        }

        public Path Edit()
        {
            route.controller = edit;

            path = origin.listManager.listProperties.segmentController.path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, origin, path.start);
        }
    }

    public class TerrainElement
    {
        Path path;
        Route route;
        EditorForm form = EditorManager.editorManager.forms[0];

        public TerrainElement(Route route, SelectionElement origin)
        {
            route = new Route(0, route.data, route.property);

            path = form.active_path.Trim(form.active_path.start + 3);
            
            path.Add(route);
        }

        public Path Enter()
        {
            return path;
        }
    }

    public class TerrainObject
    {
        Path path;
        Route route;
        EditorForm form = EditorManager.editorManager.forms[0];

        public TerrainObject(Route route, SelectionElement origin)
        {
            route = new Route(1, route.data, route.property);

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
        Route route;
        SelectionElement origin;

        List<int> enter = new List<int>() { 0 };

        public Option(Route route, SelectionElement origin)
        {
            this.route  = route;
            this.origin = origin;
        }

        public Path Enter()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(CreateRoutes(enter, route), form, origin);
        }
    }

    #endregion

    #endregion

    #endregion

    #region Methods

    static public List<Route> CreateRoutes(List<int> controllers, EditorForm form)
    {
        return CreateRoutes(controllers, new Route(form.active_path));
    }

    static public List<Route> CreateRoutes(List<int> controllers, Route route)
    {
        List<Route> routes = new List<Route>();

        foreach(int controller in controllers)
            routes.Add(new Route(controller, route.data, route.property));

        return routes;
    }

    static public Path CreatePath(List<Route> routes, EditorForm form, SelectionElement origin)
    {
        Path path   = new Path();
        path.form   = form;
        path.origin = origin;

        foreach (Route route in routes)
            path.Add(route);

        return path;
    }

    static public Path ExtendPath(Path head, List<Route> tail, SelectionElement origin)
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