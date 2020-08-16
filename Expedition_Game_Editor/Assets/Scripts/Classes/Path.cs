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
        Loaded
    }

    public List<Route> routeList;
    public EditorForm form;
    public Type type;

    public int start;

    public Path()
    {
        routeList   = new List<Route>();
    }

    public Path(List<Route> routeList, EditorForm form)
    {
        this.routeList  = routeList;
        this.form   = form;
    }

    public Path(List<Route> routeList, EditorForm form, int start)
    {
        this.routeList  = routeList;
        this.form   = form;

        this.start  = start;
    }

    #region Add
    public void Add()
    {
        //Use last used step as base
        if (routeList.Count > 0)
            Add(0, routeList.LastOrDefault().data, routeList.LastOrDefault().selectionStatus);
         else
            Add(new Route(this));
    }

    public void Add(int index)
    {
        if (routeList.Count > 0)
            Add(index, routeList.LastOrDefault().data, routeList.LastOrDefault().selectionStatus);
        else
            Add(index, new Route.Data(), Enums.SelectionStatus.Main);
    }

    public void Add(int controller, Route.Data data, Enums.SelectionStatus selectionGroup)
    {
        routeList.Add(new Route(controller, data, selectionGroup));

        data = new Route.Data();
    }

    public void Add(Route route)
    {
        this.routeList.Add(route);
    }
    #endregion

    public Path Clone()
    {
        Path path = new Path();

        routeList.ForEach(x => path.Add(x));

        path.form = form;
        path.start = start;

        path.type = type;

        return path;
    }

    public Path Trim(int step)
    {
        Path path = new Path();

        for (int i = 0; i < step; i++)
            path.routeList.Add(routeList[i]);

        path.form = form;
        path.start = start;

        path.type = type;

        return path;
    }

    public Path TrimToFirstType(Enums.DataType dataType)
    {
        var index = routeList.FindIndex(x => x.GeneralData.DataType == dataType);

        return Trim(index + 1);
    }

    public Path TrimToLastType(Enums.DataType dataType)
    {
        var index = routeList.FindLastIndex(x => x.GeneralData.DataType == dataType);

        return Trim(index + 1);
    }

    public Route FindFirstRoute(Enums.DataType dataType)
    {
        foreach(Route route in routeList)
        {
            if (route.GeneralData.DataType == dataType)
                return route;
        }

        return null;
    }

    public Route FindLastRoute(Enums.DataType dataType)
    {
        return routeList.FindLast(x => x.GeneralData.DataType == dataType);
    }

    public Route GetLastRoute()
    {
        return routeList[routeList.Count - 1];
    }

    public List<Route> CombineRoute(List<Route> addedRoute)
    {
        List<Route> newRoute = new List<Route>();
        
        foreach (Route route in routeList)
            newRoute.Add(route);

        foreach (Route route in addedRoute)
            newRoute.Add(route);

        return newRoute;
    }

    public void ReplaceAllData(Route.Data data)
    {
        routeList.ForEach(route => 
        {
            if (route.GeneralData.DataType == ((GeneralData)data.elementData).DataType)
                route.data = data;
        });
    }
}
