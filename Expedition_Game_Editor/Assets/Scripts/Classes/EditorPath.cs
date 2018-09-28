using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorPath
{
    public Path open    = new Path();
    public Path edit    = new Path();

    public EditorPath(Route route, Path path)
    {
        switch (route.data.table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(route, path);

                open = chapter.Open();
                edit = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(route, path);

                open = phase.Open();
                edit = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(route, path);

                open = quest.Open();
                edit = quest.Edit();

                break;

            case "Objective":

                PathManager.Structure objective = new PathManager.Structure(route, path);

                open = objective.Open();
                edit = objective.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(route);

                open = region.Open();
                edit = region.Edit();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(route, path);

                edit = terrain.Edit();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(route);

                open = item.Open();
                edit = item.Edit();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(route);

                open = element.Open();
                edit = element.Edit();

                break;

            default: break;
        }
    }
}
