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
        Loaded,
        Reload
    }

    public List<Route> route;
    public EditorForm form;
    public Type type;

    public int start;

    public ListManager origin;

    public Path()
    {
        route   = new List<Route>();
        form    = null;
        type    = Type.New;
        origin  = null;
    }

    public Path(List<Route> route, EditorForm form, ListManager origin)
    {
        this.route  = route;
        this.form   = form;
        this.origin = origin;

        type    = Type.New;
    }

    public Path(List<Route> route, EditorForm form, ListManager origin, int start)
    {
        this.route  = route;
        this.form   = form;
        this.origin = origin;
        this.start  = start;

        type    = Type.New;
    }

    public Path(bool new_loaded)
    {
        route   = new List<Route>();
        form    = null;

        type    = Type.New;
    }

    #region Equals

    public bool Equals(Path path)
    {
        for(int i = 0; i < route.Count; i++)
        {
            if (!route[i].Equals(path.route[i]))
                return false;
        }

        return true;
    }

    #endregion

    #region Add

    public void Add()
    {
        //Use last used step as base
        if (route.Count > 0)
            Add(0, route.LastOrDefault().data, route.LastOrDefault().selectionGroup);
         else
            Add(new Route(this));
    }

    public void Add(int index)
    {
        if (route.Count > 0)
            Add(index, route.LastOrDefault().data, route.LastOrDefault().selectionGroup);
        else
            Add(index, new Data(), Enums.SelectionGroup.Main);
    }

    public void Add(int controller, Data data, Enums.SelectionGroup selectionGroup)
    {
        route.Add(new Route(controller, data, selectionGroup));

        data = new Data();
    }

    public void Add(Route route)
    {
        this.route.Add(route.Copy());
    }

    #endregion

    public Path Copy()
    {
        Path copy = new Path();

        //Might need new route
        foreach (Route r in route)
            copy.route.Add(r);

        copy.form = form;
        copy.origin = origin;

        copy.start = start;

        copy.type = type;

        return copy;
    }

    public Path Trim(int step)
    {
        Path new_path = new Path(new List<Route>(), form, origin, start);

        for (int i = 0; i < step; i++)
            new_path.Add(route[i]);
      
        new_path.type = type;

        return new_path;
    }

    public Path TrimToLastType(Enums.DataType dataType)
    {
        var index = route.FindLastIndex(x => x.data.DataController.DataType == dataType);

        return Trim(index + 1);
    }

    public Route FindFirstRoute(Enums.DataType dataType)
    {
        foreach(Route r in route)
        {
            if (r.GeneralData().dataType == dataType)
                return r;
        }
        return null;
    }

    public Route FindLastRoute(Enums.DataType dataType)
    {
        for(int i = route.Count-1; i > 0; i--)
        {
            if (route[i].GeneralData().dataType == dataType)
                return route[i];   
        }
        return null;
    }

    public Route GetLastRoute()
    {
        return route[route.Count - 1];
    }

    public List<Route> CombineRoute(List<Route> new_route)
    {
        List<Route> route_list = new List<Route>();
        
        foreach (Route r in route)
            route_list.Add(r);

        foreach (Route r in new_route)
            route_list.Add(r);

        return route_list;
    }

    public void ReplaceAllRoutes(Data data)
    {
        foreach(Route r in route)
        {
            if (r.GeneralData().dataType == ((GeneralData)data.DataElement).dataType)
                r.data = data;
        }
    }
}
