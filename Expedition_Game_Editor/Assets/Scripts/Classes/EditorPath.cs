using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorPath
{
    public Path open    = new Path();
    public Path edit    = new Path();

    public EditorPath(ElementData data, Path path, Selection origin)
    {
        switch (data.table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(data, path, origin);

                open = chapter.Open();     
                edit = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(data, path, origin);

                open = phase.Open();
                edit = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(data, path, origin);

                open = quest.Open();
                edit = quest.Edit();

                break;

            case "Objective":

                PathManager.Structure objective = new PathManager.Structure(data, path, origin);

                open = objective.Open();
                edit = objective.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(data, origin);

                open = region.Open();
                edit = region.Edit();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(data, path, origin);

                edit = terrain.Edit();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(data, origin);

                open = item.Open();
                edit = item.Edit();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(data, origin);

                open = element.Open();
                edit = element.Edit();

                break;

            default: break;
        }
    }
}
