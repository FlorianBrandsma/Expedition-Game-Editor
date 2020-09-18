using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Route
{
    public int controllerIndex;
    public int id;
    
    public Data data { get; set; }
    public Path path;

    public Enums.SelectionStatus selectionStatus;

    public IElementData ElementData { get { return data == null ? null : data.dataList.Where(x => x.Id == id).FirstOrDefault(); } }

    public Route() { }

    public Route(int controllerIndex)
    {
        this.controllerIndex = controllerIndex;
    }

    public Route(Path path)
    {
        this.path = path;
    }

    public Route(EditorElement editorElement)
    {
        data = editorElement.DataElement.Data;
        id = editorElement.DataElement.Id;

        path = editorElement.DataElement.Path;

        selectionStatus = editorElement.selectionStatus;
    }

    public Route(int controllerIndex, Route route)
    {
        this.controllerIndex = controllerIndex;

        data = route.data;

        id = route.id;
        
        selectionStatus = route.selectionStatus;

        path = route.path;
    }

    public Route(int controllerIndex, Route route, Enums.SelectionStatus selectionStatus)
    {
        this.controllerIndex = controllerIndex;

        data = route.data;

        id = route.id;

        this.selectionStatus = selectionStatus;

        path = route.path;
    }

    public Route Clone()
    {
        var route = new Route();

        route.controllerIndex = controllerIndex;
        route.id = id;

        //if (data != null)
        //{
        //    var routeList = path.routeList;

        //    var i = routeList.IndexOf(this);

        //    if (i > 0 && routeList[i - 1].data != null)
        //    {
        //        var previousRoute = path.routeList[i - 1];

        //        if (data == routeList[i - 1].data)
        //        {
        //            Debug.Log("Copy from " + previousRoute.data.dataController);
        //            route.data = path.routeList[i - 1].data;
        //        }

        //    }
        //    else
        //    {

        //        Debug.Log("Clone from " + data.dataController);
        //        route.data = data.Clone();
        //    }
        //}

        route.path = path;

        route.selectionStatus = selectionStatus;

        return route;
    }
}
