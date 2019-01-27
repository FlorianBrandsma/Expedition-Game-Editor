using UnityEngine;
using System.Collections.Generic;

public class Path
{
    public List<Route> route    { get; set; }
    public EditorForm form      { get; set; }
    public bool loaded          { get; set; }

    //Where the controller should start walking through the path
    //after it has been extended
    public int start            { get; set; }

    //Potential improvements:
    //Remember the entire path
    //Set starting point

    public Path()
    {
        route   = new List<Route>();
        form = null;
        loaded = false;
    }

    public Path(List<Route> new_route, EditorForm new_form)
    {
        route   = new_route;
        form    = new_form;
    }

    public Path(List<Route> new_route, EditorForm new_form, int new_start)
    {
        route = new_route;
        form = new_form;
        start = new_start;
    }

    public Path(bool new_loaded)
    {
        route = new List<Route>();
        form = null;
        loaded = new_loaded;
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
        copy.loaded = loaded;

        copy.start = start;

        return copy;
    }

    public Path Trim(int step)
    {
        Path new_path = new Path(new List<Route>(), form);

        for (int i = 0; i < step; i++)
            new_path.Add(route[i]);

        new_path.start = start;

        return new_path;
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

        foreach (Route x in route)
            route_list.Add(x);

        foreach (Route x in new_route)
            route_list.Add(x);

        return route_list;
    }
}
