﻿using UnityEngine;
using System.Linq;

public class EditorPath
{
    public Path path = new Path();

    public EditorPath(SelectionElement origin)
    {
        Route route = new Route(origin);

        switch (route.GeneralData().table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = chapter.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = phase.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = quest.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = quest.Edit();

                break;

            case "Step":

                PathManager.Structure step = new PathManager.Structure(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = step.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = step.Edit();

                break;

            case "Task":

                PathManager.Structure task = new PathManager.Structure(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = task.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = task.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = region.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = region.Edit();

                if (origin.selectionProperty == SelectionManager.Property.Open)
                    path = region.Open();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            case "TerrainItem":

                PathManager.TerrainItem terrainItem = new PathManager.TerrainItem(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = terrainItem.Enter();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = item.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = item.Edit();

                if (origin.selectionProperty == SelectionManager.Property.Get)
                    path = item.Get();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = element.Enter();

                if (origin.selectionProperty == SelectionManager.Property.Edit)
                    path = element.Edit();

                if (origin.selectionProperty == SelectionManager.Property.Get)
                    path = element.Get();

                break;

            case "ObjectGraphic":

                PathManager.ObjectGraphic objectGraphic = new PathManager.ObjectGraphic(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Get)
                    path = objectGraphic.Get();

                break;

            case "Option":

                PathManager.Option option = new PathManager.Option(route, origin);

                if (origin.selectionProperty == SelectionManager.Property.Enter)
                    path = option.Enter();

            break;

            default: break;
        }
    }
}
