using UnityEngine;
using System.Linq;

public class EditorPath
{
    public Path path;

    public EditorPath(SelectionElement selection)
    {
        Route route = new Route(selection.route);

        if(selection.route.property == SelectionManager.Property.Get)
        {
            PathManager.Search search = new PathManager.Search(selection);

            path = search.Get();

            return;
        }

        switch (route.GeneralData().table)
        {
            case "Chapter":

                PathManager.Structure chapter = new PathManager.Structure(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = chapter.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = chapter.Edit();

                break;

            case "Phase":

                PathManager.Structure phase = new PathManager.Structure(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = phase.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = phase.Edit();

                break;

            case "Quest":

                PathManager.Structure quest = new PathManager.Structure(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = quest.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = quest.Edit();

                break;

            case "Objective":

                PathManager.Structure objective = new PathManager.Structure(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = objective.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = objective.Edit();

                break;

            case "TerrainElement":

                PathManager.Structure stepElement = new PathManager.Structure(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = stepElement.Enter();

                break;

            case "Task":

                PathManager.Structure task = new PathManager.Structure(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = task.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = task.Edit();

                break;

            case "Region":

                PathManager.Region region = new PathManager.Region(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = region.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = region.Edit();

                if (route.property == SelectionManager.Property.Open)
                    path = region.Open();

                break;

            case "Terrain":

                PathManager.Terrain terrain = new PathManager.Terrain(selection);

                if (route.property == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            //case "TerrainElement":

            //    PathManager.TerrainElement terrainElement = new PathManager.TerrainElement(selection);

            //    if (route.property == SelectionManager.Property.Enter)
            //        path = terrainElement.Enter();

            //    break;

            case "TerrainObject":

                PathManager.TerrainObject terrainObject = new PathManager.TerrainObject(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = terrainObject.Enter();

                break;

            case "Item":

                PathManager.Item item = new PathManager.Item(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = item.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = item.Edit();

                if (route.property == SelectionManager.Property.Get)
                    path = item.Get();

                break;

            case "Element":

                PathManager.Element element = new PathManager.Element(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = element.Enter();

                if (route.property == SelectionManager.Property.Edit)
                    path = element.Edit();

                if (route.property == SelectionManager.Property.Get)
                    path = element.Get();

                break;

            case "ObjectGraphic":

                PathManager.ObjectGraphic objectGraphic = new PathManager.ObjectGraphic(selection);

                if (route.property == SelectionManager.Property.Get)
                    path = objectGraphic.Get();

                break;

            case "Option":

                PathManager.Option option = new PathManager.Option(selection);

                if (route.property == SelectionManager.Property.Enter)
                    path = option.Enter();

            break;

            default: break;
        }
    }
}
