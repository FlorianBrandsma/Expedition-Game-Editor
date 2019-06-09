using UnityEngine;
using System.Collections;
using System.Linq;

public class Route
{
    public int controller   { get; set; }
    public Data data        { get; set; }
    public Path path        { get; set; }

    public Enums.SelectionGroup selectionGroup { get; set; }

    public Route() { }

    public Route(Path path)
    {
        controller = 0;
        data = new Data(null, new GeneralDataElement(), new[] { new SearchParameters() });
        this.path = path;
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = route.data;

        selectionGroup = route.selectionGroup;

        path = route.path;
    }

    public Route(int controller, Data data, Enums.SelectionGroup selectionGroup)
    {
        this.controller = controller;
        this.data = data;

        this.selectionGroup = selectionGroup;
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
        return (GeneralData)data.DataElement;
    }
}
