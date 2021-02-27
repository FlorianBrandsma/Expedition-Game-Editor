using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PathManager
{
    #region Paths

    #region Global
    public class Form
    {
        EditorForm form;

        List<int> controllerIndices;

        public Form(EditorForm form, List<int> controllerIndices)
        {
            this.form = form;
            this.controllerIndices = controllerIndices;
        }

        public Path Initialize()
        {
            return CreatePath(CreateRoutes(controllerIndices, form, Enums.SelectionStatus.Main, false), form);
        }
    }

    public class Dialog
    {
        EditorForm form = RenderManager.layoutManager.forms[4];

        Enums.DialogType dialogType;

        public Dialog(Enums.DialogType dialogType)
        {
            this.dialogType = dialogType;
        }

        public Path Initialize()
        {
            var controllerIndices = new List<int>() { (int)dialogType };

            return CreatePath(CreateRoutes(controllerIndices, form, Enums.SelectionStatus.Main, false), form);
        }
    }
    #endregion

    #region Portal
    public class Portal
    {
        EditorForm form = RenderManager.layoutManager.forms[1];

        List<int> source = new List<int>() { 0, 0 };

        public Path Initialize()
        {
            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main, false), form);
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
            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main, false), form);
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
            route.controllerIndex = enter;

            path.routeList.Add(route);

            return path;
        }

        public Path Edit()
        {
            route.controllerIndex = edit;

            path.routeList.Add(route);

            return path;
        }

        public Path Open()
        {
            route.controllerIndex = open;

            path.routeList.Add(route);

            return path;
        }
    }

    public class Interaction
    {
        Route route;
        DataElement dataElement;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Interaction(DataElement dataElement, Route route)
        {
            this.route = route;
            this.dataElement = dataElement;
        }

        public Path Enter()
        {
            int enter = 0;

            route.controllerIndex = enter;
            
            var pathSource = dataElement.DisplayManager.Display.DataController.SegmentController.Path;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = form,
                start = pathSource.start
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }

        public Path OutcomeList()
        {
            int enter = 1;

            route.controllerIndex = enter;

            var pathSource = dataElement.Path;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = form,
                start = pathSource.start
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }
    }

    public class Outcome
    {
        Path pathSource;
        Route route;

        int enter = 0;
        int openOutcomeScenes = 1;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Outcome(EditorElement editorElement, Route route)
        {
            this.route = route;

            pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            route.controllerIndex = enter;

            Path path = new Path()
            {
                routeList = pathSource.routeList.Concat(new List<Route>() { route }).ToList(),
                form = form,
                start = pathSource.start
            };

            return path;
        }

        public Path OpenOutcomeScenes()
        {
            route.controllerIndex = openOutcomeScenes;

            Path path = new Path()
            {
                routeList = pathSource.FindLastRoute(Enums.DataType.Interaction).path.routeList.Concat(new List<Route>() { route }).ToList(),
                form = form,
                start = pathSource.start
            };

            return path;
        }
    }

    public class Scene
    {
        Path pathSource;
        Route route;

        int enter = 0;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Scene(EditorElement editorElement, Route route)
        {
            this.route = route;

            pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            route.controllerIndex = enter;

            Path path = new Path()
            {
                routeList = pathSource.routeList.Concat(new List<Route>() { route }).ToList(),
                form = form,
                start = pathSource.start
            };

            return path;
        }
    }

    public class SceneShot
    {
        Path pathSource;
        Route route;

        int enter = 0;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public SceneShot(EditorElement editorElement, Route route)
        {
            this.route = route;

            pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            route.controllerIndex = enter;

            Path path = new Path()
            {
                routeList = pathSource.routeList.Concat(new List<Route>() { route }).ToList(),
                form = form,
                start = pathSource.start
            };

            return path;
        }
    }

    public class SceneActor
    {
        Path path;
        Route route;
        EditorForm form = RenderManager.layoutManager.forms[0];

        public SceneActor(EditorElement editorElement, Route route)
        {
            this.route = route;
        }

        public Path Open()
        {
            List<int> source = new List<int>() { (int)Enums.WorldSelectionType.Actor };

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main, false);

            var combinedRoute = form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes);

            Path path = new Path()
            {
                routeList = form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes),
                form = form,
                start = form.activePath.start,
                type = form.activePath.type
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }
    }

    public class SceneProp
    {
        Path path;
        Route route;
        EditorForm form = RenderManager.layoutManager.forms[0];

        public SceneProp(EditorElement editorElement, Route route)
        {
            this.route = route;
        }

        public Path Open()
        {
            List<int> source = new List<int>() { (int)Enums.WorldSelectionType.Prop };

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main, false);

            var combinedRoute = form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes);

            Path path = new Path()
            {
                routeList = form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes),
                form = form,
                start = form.activePath.start,
                type = form.activePath.type
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }
    }
    #endregion

    public class InteractionDestination
    {
        EditorElement editorElement;
        Path pathSource;
        Route route;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public InteractionDestination(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;

            if (editorElement.DataElement.DisplayManager != null)
                pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            List<int> open = new List<int>() { 1, (int)Enums.RegionType.InteractionDestination };

            Route customRoute = new Route(1, route, editorElement.selectionStatus, editorElement.uniqueSelection);

            pathSource = ExtendPath(route.path, CreateRoutes(open, customRoute, editorElement.selectionStatus, editorElement.uniqueSelection));
            pathSource.type = Path.Type.New;

            return pathSource;
        }

        public Path Open()
        {
            int open = 0;

            route.controllerIndex = open;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = form,
                start = pathSource.start,
                type = pathSource.type
            };

            path.routeList.ForEach(x => x.path = path);
            
            return path;
        }
    }

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

            var itemData = (ItemData)editorElement.DataElement.ElementData;

            enter   = new List<int>() { 0, 0, 0, itemData.Type };
            edit    = new List<int>() { 0, 1, 0, itemData.Type };
            get     = new List<int>() { 1 };
        }

        public Path Enter()
        {
            EditorForm form = RenderManager.layoutManager.forms[1];
            return CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
        }

        public Path Edit()
        {
            EditorForm form = RenderManager.layoutManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
        }

        public Path Get()
        {
            EditorForm form = RenderManager.layoutManager.forms[2];
            return CreatePath(CreateRoutes(get, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
        }
    }
    #endregion

    #region Interactable
    public class Interactable
    {
        EditorElement editorElement;

        Path pathSource;
        Route route;

        int enter;
        List<int> edit;

        public Interactable(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            pathSource = editorElement.DataElement.segmentController.Path;

            this.route = route;

            var interactableElementData = (InteractableElementData)editorElement.DataElement.ElementData;

            enter = 0;
            edit = new List<int>() { 0, 2, 0, interactableElementData.Type };
        }

        public Path Enter()
        {
            route.controllerIndex = enter;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = RenderManager.layoutManager.forms[0],
                type = pathSource.type
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }

        public Path Edit()
        {
            EditorForm form = RenderManager.layoutManager.forms[0];
            return CreatePath(CreateRoutes(edit, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
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

        RegionElementData regionElementData;

        int interactionDestinationController = 0;
        List<int> enter = new List<int>() { 0, 3 };
        List<int> edit = new List<int>() { 0, 4 };

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Region(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            this.route = route;

            regionElementData = (RegionElementData)route.ElementData;

            if (editorElement.DataElement.DisplayManager != null)
                path = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            //CreatePath OR ExtendPath based on route.data.type
            //Phase and Interaction extends current path
            //Base stands alone
            List<Route> routes;

            switch (regionElementData.Type)
            {
                case Enums.RegionType.Base:
                    path = CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
                    break;

                case Enums.RegionType.Phase:
                    routes = CreateRoutes(enter, route, editorElement.selectionStatus, editorElement.uniqueSelection);
                    path = ExtendPath(path, routes);
                    break;

                case Enums.RegionType.InteractionDestination:
                    route.controllerIndex = interactionDestinationController;
                    
                    path = new Path()
                    {
                        routeList = path.CombineRoute(new List<Route>() { route }),
                        form = form,
                        start = path.start
                    };

                    path.routeList.ForEach(x => x.path = path);

                    break;
            }

            path.type = Path.Type.New;

            return path;
        }

        public Path Edit()
        {
            return CreatePath(CreateRoutes(edit, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
        }

        public Path Open()
        {
            List<int> open = new List<int>() { 1, (int)regionElementData.Type };

            Route customRoute = new Route(1, route, editorElement.selectionStatus, editorElement.uniqueSelection);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, editorElement.selectionStatus, editorElement.uniqueSelection));
            path.type = Path.Type.New;

            return path;
        }

        public Path OpenSceneRegion()
        {
            List<int> open = new List<int>() { 1, (int)regionElementData.Type };

            //Only uses the data, not the index
            Route customRoute = new Route(0, route, editorElement.selectionStatus, editorElement.uniqueSelection);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, editorElement.selectionStatus, editorElement.uniqueSelection));
            
            path.type = Path.Type.New;

            return path;
        }

        public Path OpenPhaseSaveRegion()
        {
            List<int> open = new List<int>() { 0, 1, 2, 0, 0 };

            Route customRoute = new Route(1, route, editorElement.selectionStatus, editorElement.uniqueSelection);

            path = ExtendPath(route.path, CreateRoutes(open, customRoute, editorElement.selectionStatus, editorElement.uniqueSelection));

            return path;
        }
    }
    #endregion

    #region Atmosphere
    public class Atmosphere
    {
        Path pathSource;
        Route route;

        int enter = 0;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Atmosphere(EditorElement editorElement, Route route) //Combine existing path with new route
        {
            this.route = route;

            pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            route.controllerIndex = enter;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = form,
                start = pathSource.start
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }
    }
    #endregion

    #region Terrain
    public class Terrain
    {
        int edit = 0;
        int enter = 1;

        EditorElement editorElement;

        Route route;

        EditorForm form = RenderManager.layoutManager.forms[0];

        public Terrain(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;
            this.route = route;
        }

        public Path Enter()
        {
            route.controllerIndex = enter;

            var pathSource = editorElement.DataElement.Path;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = form,
                start = pathSource.start
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }

        public Path Edit()
        {
            route.controllerIndex = edit;

            var pathSource = editorElement.DataElement.segmentController.Path;

            pathSource.routeList.Add(route);

            return pathSource;
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
            route.controllerIndex = enter;

            var pathSource = editorElement.DataElement.segmentController.Path;

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(new List<Route>() { route }),
                form = form,
                type = pathSource.type
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }

        public Path Open()
        {
            List<int> source = new List<int>() { 0, 5 };

            route.controllerIndex = enter;

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main, false);
            return ExtendPath(form.activePath, routes);
        }

        public Path OpenPhaseSaveRegionWorldInteractable()
        {
            List<int> source = new List<int>() { 0, 1, 3 };

            route.controllerIndex = enter;

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main, false);
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

            List<Route> routes = CreateRoutes(source, route, Enums.SelectionStatus.Main, false);

            var combinedRoute = form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes);

            Path path = new Path()
            {
                routeList = form.activePath.TrimToLastType(Enums.DataType.Region).CombineRoute(routes),
                form = form,
                start = form.activePath.start
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
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
            return CreatePath(CreateRoutes(enter, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
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
        }

        public Path Get()
        {
            EditorForm form = RenderManager.layoutManager.forms[2];
            return CreatePath(CreateRoutes(controllers, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
        }
    }
    #endregion

    #endregion

    #region Game
    public class Game
    {
        EditorElement editorElement;
        Path pathSource;
        Route route;

        List<int> source = new List<int>() { 0, 0 };
        List<int> enter = new List<int>() { 0, 0 };

        public Game() { }

        public Game(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;
            this.route = route;

            pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Initialize()
        {
            var form = RenderManager.layoutManager.forms[3];

            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main, false), form);
        }

        public Path Enter()
        {
            var form = RenderManager.layoutManager.forms[1];

            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(CreateRoutes(enter, route, editorElement.selectionStatus, editorElement.uniqueSelection)),
                form = form,
                start = pathSource.start
            };

            path.routeList.ForEach(x => x.path = path);

            return path;
        }
    }

    public class GameMenu
    {
        EditorForm form = RenderManager.layoutManager.forms[3];
        
        public Path OpenSaveMenu(Enums.SaveType saveType)
        {
            List<int> source = new List<int>() { 0, ((int)saveType + 1) };

            return CreatePath(CreateRoutes(source, form, Enums.SelectionStatus.Main, false), form);
        }
    }

    public class Save
    {
        EditorElement editorElement;
        Path pathSource;
        Route route;

        List<int> enter = new List<int>() { 0, 0 };
        List<int> open  = new List<int>() { 0 };

        public Save(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;
            this.route = route;

            pathSource = editorElement.DataElement.segmentController.Path;
        }

        public Path Enter()
        {
            Path path = new Path()
            {
                routeList = pathSource.CombineRoute(CreateRoutes(enter, route, editorElement.selectionStatus, editorElement.uniqueSelection)),
                form = RenderManager.layoutManager.forms[3],
                start = pathSource.start
            };

            path.routeList.ForEach(x => x.path = path);
            
            return path;
        }

        public Path Open()
        {
            RenderManager.CloseForms();

            HistoryManager.ClearHistory();

            var form = RenderManager.layoutManager.forms[0];

            var path = CreatePath(CreateRoutes(open, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
            path.type = Path.Type.New;

            return path;
        }
    }

    public class InteractableSave
    {
        EditorElement editorElement;

        Path path;
        Route route;

        public InteractableSave(EditorElement editorElement, Route route)
        {
            this.editorElement = editorElement;

            path = editorElement.DataElement.segmentController.Path;

            this.route = route;
        }

        public Path Edit()
        {
            EditorForm form = RenderManager.layoutManager.forms[0];

            var controllers = new List<int> { 0, 1, 1, 0 };

            Path newPath = CreatePath(CreateRoutes(controllers, route, editorElement.selectionStatus, editorElement.uniqueSelection), form);
            newPath.type = path.type;

            return newPath;
        }
    }

    #endregion

    #endregion

    #region Methods
    static public List<Route> CreateRoutes(List<int> controllers, EditorForm form, Enums.SelectionStatus selectionStatus, bool uniqueSelection)
    {
        return CreateRoutes(controllers, new Route(form.activePath), selectionStatus, uniqueSelection);
    }

    static public List<Route> CreateRoutes(List<int> controllers, Route route, Enums.SelectionStatus selectionStatus, bool uniqueSelection)
    {
        List<Route> routes = new List<Route>();

        foreach(int controller in controllers)
            routes.Add(new Route(controller, route, selectionStatus, uniqueSelection));

        return routes;
    }

    static public Path CreatePath(List<Route> routes, EditorForm form)
    {
        Path path   = new Path();
        path.form   = form;

        foreach (Route route in routes)
        {
            path.Add(route);
        }

        return path;
    }

    static public Path ExtendPath(Path head, List<Route> tail)
    {
        Path path = new Path()
        {
            routeList = head.CombineRoute(tail),
            form = head.form,
            start = head.routeList.Count,
            type = head.type
        };

        path.routeList.ForEach(x => x.path = path);
        
        return path;
    }
    #endregion
}