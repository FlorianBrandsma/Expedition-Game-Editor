using UnityEngine;
using System.Collections.Generic;

public class Path
{
    public enum Type
    {
        New,
        Previous,
        Loaded,
        Reload
    }

    public List<Route> route    { get; set; }
    public EditorForm form      { get; set; }
    public Type type { get; set; }

    public int start            { get; set; }

    public Path()
    {
        route   = new List<Route>();
        form    = null;
        type    = Type.New;
    }

    public Path(List<Route> new_route, EditorForm new_form)
    {
        route   = new_route;
        form    = new_form;

        type    = Type.New;
    }

    public Path(List<Route> new_route, EditorForm new_form, int new_start)
    {
        route   = new_route;
        form    = new_form;
        start   = new_start;

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
            Add(0, route[route.Count - 1].data, route[route.Count - 1].origin);
        else
            Add(new Route(this));
    }

    public void Add(int index)
    {
        if (route.Count > 0)
            Add(index, route[route.Count - 1].data, route[route.Count - 1].origin);
        else
            Add(index, new ElementData(), new Origin());
    }

    public void Add(int new_controller, ElementData new_data, Origin new_origin)
    {
        route.Add(new Route(new_controller, new_data, new_origin));
    }

    public void Add(Route new_route)
    {
        route.Add(new_route.Copy());
    }

    #endregion

    public Path Copy()
    {
        Path copy = new Path();

        //Might need new route
        foreach (Route x in route)
            copy.route.Add(x);

        copy.form = form;

        copy.start = start;

        copy.type = type;

        return copy;
    }

    public Path Trim(int step)
    {
        Path new_path = new Path(new List<Route>(), form);

        for (int i = 0; i < step; i++)
            new_path.Add(route[i]);

        new_path.start = start;

        new_path.type = type;

        return new_path;
    }

    public Route FindFirstRoute(string table)
    {
        foreach(Route r in route)
        {
            if (r.data.table == table)
                return r;
        }
        return null;
    }

    public Route FindLastRoute(string table)
    {
        for(int i = route.Count-1; i > 0; i--)
        {
            if (route[i].data.table == table)
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

    public void ReplaceAllRoutes(ElementData data)
    {
        foreach(Route r in route)
        {
            if (r.data.table == data.table)
                r.data = data;
        }
    }
}
