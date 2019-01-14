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
            return CreatePath(source, form);
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
            return CreatePath(source, form);
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

        int open = 0;
        int edit = 1;

        EditorForm form  = EditorManager.editorManager.forms[0];

        public Structure(Route new_route) //Combine existing path with new route
        {
            route = new_route;

            path = route.origin.listManager.listData.controller.path;
        }

        public Path Open()
        {
            route.controller = open;
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
        List<int> open;
        List<int> edit;
        List<int> get;

        Route route;

        public Item(Route new_route)
        {
            route = new_route;

            open    = new List<int>() { 0, 0, 0, route.data.type };
            edit    = new List<int>() { 0, 1, 0, route.data.type };
            get     = new List<int>() { 1 };
        }

        public Path Open()
        {
            EditorForm form = EditorManager.editorManager.forms[1];
            return CreatePath(open, route, form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(edit, route, form);
        }

        public Path Get()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(get, route, form);
        }
    }

    #endregion

    #region Element

    public class Element
    {
        int open;
        List<int> edit;

        Route route;
        Path path;

        public Element(Route new_route)
        {
            route = new_route;

            open    = 0;
            edit    = new List<int>() { 0, 1, 1, new_route.data.type };
        }

        public Path Open()
        {
            route.controller = open;

            path = route.origin.listManager.listData.controller.path;

            EditorForm form = EditorManager.editorManager.forms[0];
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(edit, route, form);
        }

        public Path Get()
        {
            return null;
        }
    }

    #endregion

    #region Region

    public class Region
    {
        List<int> open  = new List<int>() { 0, 2 };
        List<int> edit  = new List<int>() { 0, 3 };

        Route route;

        public Region(Route new_route)
        {
            route = new_route;
        }

        public Path Open()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(open, route, form);
        }

        public Path Edit()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(edit, route, form);
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        int edit = 0;
        List<int> open;

        Route route;
        Path path;

        EditorForm form = EditorManager.editorManager.forms[0];

        public Terrain(Route new_route)
        {
            route = new_route;

            open = new List<int>() { 1, route.data.type, 0 };
        }

        //First step: Default 

        public Path Open()
        {
            EditorForm form = EditorManager.editorManager.forms[0];
            return CreatePath(open, route, form);
        }

        public Path Edit()
        {
            route.controller = edit;

            path = route.origin.listManager.listData.controller.path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form);
        }
    }

    public class TerrainItem
    {
        List<int> open;

        Route route;

        EditorForm form = EditorManager.editorManager.forms[0];

        public TerrainItem(Route new_route)
        {
            route = new_route;
            //route[1] is hardcoded. Could be dangerous?
            open = new List<int>() { 1, form.active_path.route[1].controller, 0, route.data.type };
        }

        public Path Open()
        {
            return CreatePath(open, route, form);
        }
    }

    #endregion

    #region Options

    public class Option
    {
        List<int> open = new List<int>() { 0 };

        Route route;

        public Option(Route new_route)
        {
            route = new_route;
        }

        public Path Open()
        {
            EditorForm form = EditorManager.editorManager.forms[2];
            return CreatePath(open, route, form);
        }
    }

    #endregion

    #endregion

    #endregion

    #region Functions

    static public Path CreatePath(List<int> new_controllers, EditorForm new_form)
    {
        return CreatePath(new_controllers, new Route(), new_form);
    }

    static public Path CreatePath(List<int> new_controllers, Route new_route, EditorForm new_form)
    {
        Path path = new Path();

        path.form = new_form;

        for (int i = 0; i < new_controllers.Count; i++)
            path.route.Add(new Route(new_controllers[i], new_route.data, new_route.origin));

        return path;
    }

    static public Path ReloadPath(Path new_path, ElementData new_data)
    {
        Path path = new Path();

        path.form = new_path.form;

        foreach (Route route in new_path.route)
            path.Add(route);

        path.route.Last().data = new_data;

        return path;
    }

    #endregion
}