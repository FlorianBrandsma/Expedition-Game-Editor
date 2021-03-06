﻿using UnityEngine;
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

    public List<Route> routeList = new List<Route>();
    public EditorForm form;
    public Type type;

    public Enums.HistoryGroup historyGroup;

    public int start;

    #region Add
    public void Add()
    {
        //Use last used step as base
        if (routeList.Count > 0)
            Add(0, routeList.LastOrDefault());
         else
            Add(new Route(this));
    }

    public void Add(int controllerIndex)
    {
        if (routeList.Count > 0)
            Add(controllerIndex, routeList.LastOrDefault());
        else
            Add(new Route(controllerIndex));
    }

    public void Add(int controller, Route route /*Data data, Enums.SelectionStatus selectionGroup*/)
    {
        routeList.Add(new Route(controller, route));
    }

    public void Add(Route route)
    {
        routeList.Add(route);
        route.path = this;
    }
    #endregion

    public Path Trim(int step)
    {
        Path path = new Path();

        for (int i = 0; i < step; i++)
        {
            path.routeList.Add(routeList[i]);
        }

        path.form = form;
        path.start = start;

        path.type = type;

        path.historyGroup = historyGroup;

        return path;
    }

    public Path TrimToFirstType(Enums.DataType dataType)
    {
        var index = routeList.FindIndex(x => x.ElementData.DataType == dataType);

        return Trim(index + 1);
    }

    public Path TrimToLastType(Enums.DataType dataType)
    {
        var index = routeList.FindLastIndex(x => x.ElementData.DataType == dataType);

        return Trim(index + 1);
    }

    public Route FindFirstRoute(Enums.DataType dataType)
    {
        return routeList.Where(x => x.data != null).ToList().Find(x => x.data.dataController.DataType == dataType);
    }

    public Route FindLastRoute(Enums.DataType dataType)
    {
        return routeList.Where(x => x.data != null).ToList().FindLast(x => x.data.dataController.DataType == dataType);
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

    public Path Clone()
    {
        var path = new Path();
        
        path.routeList = new List<Route>();

        for(int i = 0; i < routeList.Count; i++)
        {
            var route = new Route();

            var originalRoute = routeList[i];

            route.controllerIndex = originalRoute.controllerIndex;
            route.id = originalRoute.id;

            if (originalRoute.data != null)
            {
                var originalRouteList = originalRoute.path.routeList;
                
                if(i > 0 && originalRoute.data == originalRouteList[i - 1].data)
                {
                    //Debug.Log("Copy from " + path.routeList[i - 1].data.dataController);
                    route.data = path.routeList[i - 1].data;
                    
                } else {

                    //Debug.Log("Clone from " + originalRoute.data.dataController);
                    route.data = originalRoute.data.Clone();
                }
            }

            route.path = path;

            route.selectionStatus = originalRoute.selectionStatus;
            route.uniqueSelection = originalRoute.uniqueSelection;
            
            path.routeList.Add(route);
        }

        path.form = form;
        path.type = type;

        path.historyGroup = historyGroup;

        path.start = start;
        
        return path;
    }
}
