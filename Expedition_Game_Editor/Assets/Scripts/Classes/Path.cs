using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path
{
    public enum Type
    {
        New,
        Previous,
        Loaded
    }

    public List<Route> route;
    public EditorForm form;
    public Type type;

    public int start;

    public Path()
    {
        route   = new List<Route>();
    }

    public Path(List<Route> route, EditorForm form)
    {
        this.route  = route;
        this.form   = form;
    }

    public Path(List<Route> route, EditorForm form, int start)
    {
        this.route  = route;
        this.form   = form;

        this.start  = start;
    }

    #region Add

    public void Add()
    {
        //Use last used step as base
        if (route.Count > 0)
            Add(0, route.LastOrDefault().data, route.LastOrDefault().selectionStatus);
         else
            Add(new Route(this));
    }

    public void Add(int index)
    {
        if (route.Count > 0)
            Add(index, route.LastOrDefault().data, route.LastOrDefault().selectionStatus);
        else
            Add(index, new Route.Data(), Enums.SelectionStatus.Main);
    }

    public void Add(int controller, Route.Data data, Enums.SelectionStatus selectionGroup)
    {
        route.Add(new Route(controller, data, selectionGroup));

        data = new Route.Data();
    }

    public void Add(Route route)
    {
        this.route.Add(route);
    }

    #endregion

    public Path Trim(int step)
    {
        Path path = new Path();

        for (int i = 0; i < step; i++)
            path.route.Add(route[i]);

        path.form = form;
        path.start = start;

        path.type = type;

        return path;
    }

    public Path TrimToLastType(Enums.DataType dataType)
    {
        var index = route.FindLastIndex(x => x.GeneralData.DataType == dataType);

        return Trim(index + 1);
    }

    public Route FindFirstRoute(Enums.DataType dataType)
    {
        foreach(Route r in route)
        {
            if (r.GeneralData.DataType == dataType)
                return r;
        }

        return null;
    }

    public Route FindLastRoute(Enums.DataType dataType)
    {
        return route.FindLast(x => x.GeneralData.DataType == dataType);
    }

    public Route GetLastRoute()
    {
        return route[route.Count - 1];
    }

    public List<Route> CombineRoute(List<Route> addedRoute)
    {
        List<Route> newRoute = new List<Route>();
        
        foreach (Route r in route)
            newRoute.Add(new Route(r));

        foreach (Route r in addedRoute)
            newRoute.Add(new Route(r));

        return newRoute;
    }

    public void ReplaceAllRoutes(Route.Data data)
    {
        foreach(Route r in route)
        {
            if (r.GeneralData.DataType == ((GeneralData)data.dataElement).DataType)
                r.data = data;
        }
    }
}
