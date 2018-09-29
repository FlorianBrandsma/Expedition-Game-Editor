using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathManager
{
    #region Paths

    #region Initializers

    #region Primary

    public class Primary
    {
        SectionManager section = EditorManager.editorManager.sections[0];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(source, section);
        }
    }

    #endregion

    #region Secondary

    public class Secondary
    {
        SectionManager section = EditorManager.editorManager.sections[1];

        List<int> source = new List<int>() { 0 };

        public Path Initialize()
        {
            return CreatePath(source, section);
        }
    }

    #endregion

    #region Tertiary

    public class Tertiary
    {
        SectionManager section = EditorManager.editorManager.sections[2];

        List<int> open = new List<int>() { };

        public Path Initialize()
        {
            return CreatePath(open, section);
        }
    }

    #endregion

    #endregion

    #region Editor

    #region Structure
    public class Structure
    {
        Route route = new Route();
        Path path = new Path();

        int open = 0;
        int edit = 1;

        SectionManager section  = EditorManager.editorManager.sections[0];

        public Structure(Route new_route) //Combine existing path with new route
        {
            route = new_route;
            path = route.origin.listManager.listData.controller.path;
        }

        public Path Open()
        {
            route.controller = open;
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), section);
        }

        public Path Edit()
        {
            route.controller = edit;
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), section);
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

            open    = new List<int>() { 0, 0, route.data.type };
            edit    = new List<int>() { 0, 3, 0, route.data.type };
            get     = new List<int>() { 0, 1 };
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[1];
            return CreatePath(open, route, section);
        }

        public Path Edit()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(edit, route, section);
        }

        public Path Get()
        {
            SectionManager section = EditorManager.editorManager.sections[1];
            return CreatePath(get, route, section);
        }
    }

    #endregion

    #region Element

    public class Element
    {
        List<int> open;
        List<int> edit;

        Route route;

        public Element(Route new_route)
        {
            route = new_route;

            open    = new List<int>() { 0, 1, new_route.data.type };
            edit    = new List<int>() { 0, 3, 1, new_route.data.type };
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[1];
            return CreatePath(open, route, section);
        }

        public Path Edit()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(edit, route, section);
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
        List<int> edit  = new List<int>() { 0, 1, 0 };

        Route route;

        public Region(Route new_route)
        {
            route = new_route;
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(open, route, section);
        }

        public Path Edit()
        {
            SectionManager section = EditorManager.editorManager.sections[0];
            return CreatePath(edit, route, section);
        }
    }

    #endregion

    #region Terrain

    public class Terrain
    {
        int edit = 0;

        Path path   = new Path();
        Route route = new Route();

        SectionManager section = EditorManager.editorManager.sections[0];

        public Terrain(Route new_route)
        {
            route = new_route;
            path = route.origin.listManager.listData.controller.path;
        }

        public Path Edit()
        {
            route.controller = edit;

            return new Path(path.CombineRoute(new List<Route>() { route }), section);
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
            SectionManager section = EditorManager.editorManager.sections[2];
            return CreatePath(open, route, section);
        }
    }

    #region Assets (Temporary)

    public class Asset
    {
        List<int> open;

        Route route;

        public Asset(Route new_route)
        {
            route   = new_route;

            open    = new List<int>() { new_route.data.type };
        }

        public Path Open()
        {
            SectionManager section = EditorManager.editorManager.sections[1];
            return CreatePath(open, route, section);
        }
    }

    #endregion

    #endregion

    #endregion

    #endregion
    static public Path CreatePath(List<int> new_controllers, SectionManager new_section)
    {
        return CreatePath(new_controllers, new Route(), new_section);
    }

    static public Path CreatePath(List<int> new_controllers, Route new_route, SectionManager new_section)
    {
        Path path = new Path();

        path.section = new_section;

        for (int i = 0; i < new_controllers.Count; i++)
            path.route.Add(new Route(new_controllers[i], new_route.data, new_route.origin));

        return path;
    }

    static public Path ReloadPath(Path new_path, ElementData new_data)
    {
        Path path = new Path();

        path.section = new_path.section;

        foreach (Route route in new_path.route)
            path.Add(route);

        path.route.Last().data = new_data;

        return path;
    }
}