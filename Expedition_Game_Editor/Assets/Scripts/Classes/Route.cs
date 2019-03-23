using UnityEngine;
using System.Collections;
using System.Linq;

public class Route
{
    public int controller               { get; set; }
    public IEnumerable data             { get; set; }
    public DataManager.Type data_type   { get; set; }

    public Path path                    { get; set; }

    public Route(Path new_path)
    {
        controller = 0;
        data = new[] { new GeneralData() };
        data_type = DataManager.Type.None;
        path = new_path;
    }

    public Route(Route route)
    {
        controller = route.controller;
        data = route.data;
        data_type = route.data_type;
        path = route.path;
    }

    public Route(int new_controller, IEnumerable new_data, DataManager.Type new_data_type)
    {
        controller = new_controller;
        data = new_data;
        data_type = new_data_type;
    }

    public Route(SelectionElement selection)
    {
        controller = 0;
        data = selection.data; //Copy?
        data_type = selection.data_type;

        //TEMPORARY
        if(selection.segmentController != null)
            path = selection.segmentController.path;
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
