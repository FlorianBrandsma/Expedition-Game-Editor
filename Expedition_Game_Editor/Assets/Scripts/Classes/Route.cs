using UnityEngine;
using System.Collections;
using System.Linq;

public class Route
{
    public int controller               { get; set; }
    public IEnumerable data             { get; set; }
    public Enums.DataType data_type     { get; set; }

    public Path path                    { get; set; }

    public SelectionManager.Property property  { get; set; }

    public Route() { }

    public Route(Path new_path)
    {
        controller = 0;
        data = new[] { new GeneralData() };
        data_type = Enums.DataType.None;
        path = new_path;
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = route.data;
        data_type = route.data_type;

        property = route.property;

        path = route.path;
    }

    public Route(int controller, IEnumerable data, Enums.DataType data_type, SelectionManager.Property property)
    {
        this.controller = controller;
        this.data = data;
        this.data_type = data_type;

        this.property = property;
    }

    public bool Equals(Route new_route)
    {
        if (controller != new_route.controller)
            return false;

        if (!GeneralData().Equals(new_route.GeneralData()))
            return false;

        return true;
    }

    public Route Copy()
    {
        return new Route(this);
    }

    public GeneralData GeneralData()
    {
        return data.Cast<GeneralData>().FirstOrDefault();
    }
}
