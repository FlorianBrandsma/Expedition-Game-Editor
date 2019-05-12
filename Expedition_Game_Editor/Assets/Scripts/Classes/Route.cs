using UnityEngine;
using System.Collections;
using System.Linq;

public class Route
{
    public int controller   { get; set; }
    public Data data        { get; set; }
    public Path path        { get; set; }

    public SelectionManager.Property property   { get; set; }

    public Route() { }

    public Route(Path path)
    {
        controller = 0;
        data = new Data(null, new[] { new GeneralData() }, new[] { new SearchParameters() });
        this.path = path;
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = route.data;

        property = route.property;

        path = route.path;
    }

    public Route(int controller, Data data, SelectionManager.Property property)
    {
        this.controller = controller;
        this.data = data;

        this.property = property;
    }

    public bool Equals(Route route)
    {
        if (controller != route.controller)
            return false;

        if (!GeneralData().Equals(route.GeneralData()))
            return false;

        return true;
    }

    public Route Copy()
    {
        return new Route(this);
    }

    public GeneralData GeneralData()
    {
        return data.ElementData.Cast<GeneralData>().FirstOrDefault();
    }
}
