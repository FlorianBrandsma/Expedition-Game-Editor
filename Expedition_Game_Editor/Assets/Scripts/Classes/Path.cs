using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Path
{
    public List<Route> route        { get; set; }
    //public List<int> route          { get; set; }
    //public List<ElementData> data   { get; set; }
    public SectionManager section   { get; set; }
    //public List<Selection> origin   { get; set; }

    public Path()
    {
        route   = new List<Route>();
        section = null;
    }

    public Path(List<Route> new_route, SectionManager new_section)
    {
        route   = new_route;
        section = new_section;
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
            Add(0, new ElementData(), new Origin());
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

        copy.section = section;

        return copy;
    }

    //--SOON OBSELETE : ONLY USED BY MINI BUTTONS!
    //Create a new path based on an editor list. ID list is generated
    //Totally useless
    public Path CreateEdit(List<int> base_route)
    {
        List<int> base_id = new List<int>();

        for (int i = 0; i < base_route.Count; i++)
            base_id.Add(0);

        return CreateEdit(base_route, base_id);
    }

    //Create a new path based on editor and id lists
    public Path CreateEdit(List<int> base_route, List<int> base_id)
    {
        Path path = new Path();

        for (int i = 0; i < base_route.Count; i++)
            path.Add(base_route[i]);

        path.section = section;

        return path;
    }
    //--

    public Path Trim(int step)
    {
        Path new_path = new Path(new List<Route>(), section);

        for (int i = 0; i < step; i++)
            new_path.Add(route[i]);

        return new_path;
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
