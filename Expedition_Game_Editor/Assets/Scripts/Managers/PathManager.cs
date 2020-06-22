using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathManager
{
    #region Paths

    #region Global
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
            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main), form);
        }
    }
    #endregion

    #region Editor
    public class Editor
    {
        EditorForm form = RenderManager.layoutManager.forms[0];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main), form);
        }
    }

    #region Structure
    public class Structure
    {
        Path path;
        Route route;

        int enter = 0;
        int edit = 1;
        int open = 0;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Structure(EditorElement editorElement, Route route) //Combine existing path with new route
        {
            this.route = route;

            path = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            route.controller = enter;

            Path newPath = new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
            newPath.type = path.type;

            return newPath;
        }

        public Path Edit()
        {
            route.controller = edit;

            Path newPath = new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, path.start);
            newPath.type = path.type;

            return newPath;
        }

        public Path Open()
        {
            route.controller = open;

            Path newPath = new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, path.start);
            newPath.type = path.type;

            return newPath;
        }
    }

    public class Interaction
    {
        Path path;
        Route route;
        DataElement selectionElement;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Interaction(DataElement selection, Route route)
        {
            this.route = route;
            selectionElement = selection;
        }

        public Path Enter()
        {
            int enter = 0;

            route.controller = enter;

            path = selectionElement.DisplayManager.Display.DataController.SegmentController.Path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }

        public Path OutcomeList()
        {
            int enter = 1;

            route.controller = enter;

            path = selectionElement.Path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }
    }

    public class Outcome
    {
        Path path;
        Route route;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Outcome(DataElement selection, Route route)
        {
            this.route = route;

            path = selection.DisplayManager.Display.DataController.SegmentController.Path;
        }

        public Path OutcomeEditor()
        {
            int open = 0;

            route.controller = open;

            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form, path.start);
        }
    }

    #endregion

    #region Item
    public class Item
    {
        EditorElement editorElement;

        Route route;

        List<int> enter;
        List<int> edit;
        List<int> get;

        public Item(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;

            var itemDataElement = (ItemDataElement)editorElement.DataElement.data.dataElement;

            enter   = new List<int>() { 0, 0, 0, itemDataElement.Type };
            edit    = new List<int>() { 0, 1, 0, itemDataElement.Type };
            get     = new List<int>() { 1 };
        }

        public Path Enter()
        {
            EditorForm form = RenderManager.layoutManager.forms[1];
            return CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus), form);
        }

        public Path Edit()
        {
            EditorForm form = RenderManager.layoutManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, editorElement.selectionStatus), form);
        }

        public Path Get()
        {
            EditorForm form = RenderManager.layoutManager.forms[2];
            return CreatePath(CreateRoutes(get, route, editorElement.selectionStatus), form);
        }
    }
    #endregion

    #region Interactable
    public class Interactable
    {
        EditorElement editorElement;

        Path path;
        Route route;

        int enter;
        List<int> edit;

        public Interactable(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            path = editorElement.DataElement.segmentController.Path;

            this.route = route;

            var interactableDataElement = (InteractableDataElement)editorElement.DataElement.data.dataElement;

            enter = 0;
            edit = new List<int>() { 0, 2, 0, interactableDataElement.Type };
        }

        public Path Enter()
        {
            route.controller = enter;
            //Looks convoluted
            
            EditorForm form = RenderManager.layoutManager.forms[0];
            return new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
        }

        public Path Edit()
        {
            EditorForm form = RenderManager.layoutManager.forms[0];

            return CreatePath(CreateRoutes(edit, route, editorElement.selectionStatus), form);
        }

        public Path OpenDataCharacters()
        {
            EditorForm form = RenderManager.layoutManager.forms[0];

            var controllers = new List<int> { 0, 1, 1, 0 };

            Path newPath = CreatePath(CreateRoutes(controllers, route, editorElement.selectionStatus), form);
            newPath.type = path.type;

            return newPath;
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
        EditorElement editorElement;
        Path path;
        Route route;

        RegionDataElement regionDataElement;

        List<int> enter = new List<int>() { 0, 3 };
        List<int> edit = new List<int>() { 0, 4 };

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Region(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;

            regionDataElement = (RegionDataElement)route.data.dataElement;

            if (editorElement.DataElement.DisplayManager != null)
                path = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            //CreatePath OR ExtendPath based on route.data.type
            //Phase and Interaction extends current path
            //Base stands alone
            List<Route> routes;

            switch (regionDataElement.type)
            {
                case Enums.RegionType.Base:
                    path = CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus), form);
                    break;
                case Enums.RegionType.Phase:
                    routes = CreateRoutes(enter, route, editorElement.selectionStatus);
                    path = ExtendPath(path, routes);
                    break;
            }

            path.type = Path.Type.New;

            return path;
        }

        public Path Edit()
        {
            return CreatePath(CreateRoutes(edit, route, editorElement.selectionStatus), form);
        }

        public Path Open()
        {
            List<int> open = new List<int>() { 1, (int)regionDataElement.type };

            Route customRoute = new Route(1, route.data, editorElement.selectionStatus);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, editorElement.selectionStatus));
            path.type = Path.Type.New;

            return path;
        }

        public Path OpenPhaseSaveRegion()
        {
            List<int> open = new List<int>() { 0, 1, 2, 0, 0 };

            Route customRoute = new Route(1, route.data, editorElement.selectionStatus);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, editorElement.selectionStatus));

            return path;
        }
    }

    #endregion

    #region Atmosphere
    public class Atmosphere
    {
        Path path;
        Route route;

        int enter = 0;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Atmosphere(EditorElement editorElement, Route route) //Combine existing path with new route
        {
            this.route = route;

            path = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            route.controller = enter;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }
    }
    #endregion

    #region Terrain

    public class Terrain
    {
        int edit = 0;
        int enter = 1;

        EditorElement editorElement;
        Path path;
        Route route;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Terrain(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;
            this.route = route;
        }

        public Path Enter()
        {
            route.controller = enter;

            path = editorElement.DataElement.Path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }

        public Path Edit()
        {
            route.controller = edit;

            path = editorElement.DataElement.segmentController.Path;

            return new Path(path.CombineRoute(new List<Route>() { route }), form, path.start);
        }
    }

    public class WorldInteractable
    {
        EditorElement editorElement;
        Path path;
        Route route;
        EditorForm form = RenderManager.layoutManager.forms[0];

        int enter = 0;

        public WorldInteractable(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;
        }

        public Path Enter()
        {
            route.controller = enter;

            path = editorElement.DataElement.segmentController.Path;

            Path newPath = new Path(path.CombineRoute(new List<Route>() { new Route(route) }), form);
            newPath.type = path.type;

            return newPath;
        }

        public Path Open()
        {
            List<int> source = new List<int>() { 0, 5 };

            route.controller = enter;

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main);
            return ExtendPath(form.activePath, routes);
        }

        public Path OpenPhaseSaveRegionWorldInteractable()
        {
            List<int> source = new List<int>() { 0, 1, 3 };

            route.controller = enter;

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main);
            return ExtendPath(form.activePath, routes);
        }
    }

    public class WorldObject
    {
        Path path;
        Route route;
        EditorForm form = RenderManager.layoutManager.forms[0];

        public WorldObject(EditorElement editorElement, Route route)
        {
            this.route = route;
        }

        public Path Open()
        {
            List<int> source = new List<int>() { (int)Enums.WorldSelectionType.Object };

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main);

            return new Path(form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes), form, form.activePath.start);
        }
    }

    #endregion

    #region Options
    public class Option
    {
        EditorElement editorElement;
        Route route;

        List<int> enter = new List<int>() { 0 };

        public Option(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;
        }

        public Path Enter()
        {
            EditorForm form = RenderManager.layoutManager.forms[2];
            return CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus), form);
        }
    }
    #endregion

    #region Search
    public class Search
    {
        EditorElement editorElement;
        Route route;

        List<int> controllers = new List<int>() { 1 };

        public Search(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;
            this.route = route;

            route.data = new Route.Data(route.data.dataController, route.data.dataElement, editorElement.DataElement.data.dataController.SearchProperties);
        }

        public Path Get()
        {
            EditorForm form = RenderManager.layoutManager.forms[2];
            return CreatePath(CreateRoutes(controllers, route, editorElement.selectionStatus), form);
        }
    }
    #endregion

    #endregion

    #region Game
    public class Game
    {
        EditorForm form = RenderManager.layoutManager.forms[3];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main), form);
        }
    }

    public class GameMenu
    {
        EditorForm form = RenderManager.layoutManager.forms[3];
        
        List<int> source = new List<int>() { 0, 1 };
        
        public Path LoadSave()
        {
            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main), form);
        }
    }

    public class Save
    {
        EditorElement editorElement;
        Path path;
        Route route;

        List<int> enter = new List<int>() { 0 };

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Save(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;

            if (editorElement.DataElement.DisplayManager != null)
                path = editorElement.DataElement.segmentController.Path;
        }

        public Path EnterGame()
        {
            RenderManager.CloseForms();

            HistoryManager.ClearHistory();
            
            path = CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus), form);
            path.type = Path.Type.New;

            return path;
        }
    }
    #endregion

    #endregion

    #region Methods
    static public List<Route> CreateRoutes(List<int> controllers, EditorForm form, Enums.SelectionStatus selectionStatus)
    {
        return CreateRoutes(controllers, new Route(form.activePath), selectionStatus);
    }

    static public List<Route> CreateRoutes(List<int> controllers, Route route, Enums.SelectionStatus selectionStatus)
    {
        List<Route> routes = new List<Route>();

        foreach(int controller in controllers)
            routes.Add(new Route(controller, route.data, selectionStatus));

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

    static public Path ReloadPath(Path path, Route.Data data)
    {
        var newPath = new Path();

        newPath.form = path.form;

        path.route.ForEach(x => newPath.Add(x.Copy()));

        newPath.route.Last().data = data;

        newPath.start = path.start;

        newPath.type = path.type;

        RenderManager.loadType = Enums.LoadType.Reload;

        return newPath;
    }
    #endregion
}