using UnityEngine;
using System.Collections;

public class Route
{
    public int controller { get; set; }
    public ElementData data { get; set; }
    public Origin origin { get; set; }

    public Route()
    {
        controller = 0;
        data = new ElementData();
        origin = new Origin();
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = route.data;
        origin = route.origin;
    }

    public Route(int new_controller, ElementData new_data, Origin new_origin)
    {
        controller = new_controller;
        data = new_data;
        origin = new_origin;
    }

    public Route(SelectionElement selection)
    {
        controller = 0;
        data = selection.data.Copy();
        origin = new Origin(selection);
    }

    public bool Equals(Route new_route)
    {
        if (controller != new_route.controller)
            return false;

        if (!data.Equals(new_route.data))
            return false;

        return true;
    }

    public Route Copy()
    {
        return new Route(this);
    }
}
