using UnityEngine;
using System.Linq;

public class EditorPath
{
    public Path path;

    public EditorPath(SelectionElement selection)
    {
        Route route = new Route(selection.route);

        if(selection.selectionProperty == SelectionManager.Property.Get)
        {
            PathManager.Search search = new PathManager.Search(selection);

            path = search.Get();

            return;
        }

        switch (route.GeneralData().dataType)
        {
            case Enums.DataType.Chapter:

                PathManager.Structure chapter = new PathManager.Structure(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = chapter.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = chapter.Edit();

                break;

            case Enums.DataType.Phase:

                PathManager.Structure phase = new PathManager.Structure(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = phase.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = phase.Edit();

                break;

            case Enums.DataType.Quest:

                PathManager.Structure quest = new PathManager.Structure(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = quest.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = quest.Edit();

                break;

            case Enums.DataType.Objective:

                PathManager.Structure objective = new PathManager.Structure(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = objective.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = objective.Edit();

                break;

            case Enums.DataType.TerrainElement:

                //Don't stick with "Enter/Edit/Etc" restrictions
                //Add Combine, Extend: somewhat global, so not like "TerrainElementExtend"

                //Add another path to root, to directly open Task without going through the path
                //The way an asset-less Task opens the selection is kind of a happy accident, but something more intentional would be preferable

                PathManager.TerrainElement terrainElement = new PathManager.TerrainElement(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = terrainElement.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = terrainElement.Open();
                
                break;

            case Enums.DataType.Task:

                PathManager.Structure task = new PathManager.Structure(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = task.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = task.Edit();

                break;

            case Enums.DataType.Region:

                PathManager.Region region = new PathManager.Region(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = region.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = region.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = region.Open();

                break;

            case Enums.DataType.Terrain:

                PathManager.Terrain terrain = new PathManager.Terrain(selection);

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            //case "TerrainElement":

            //    PathManager.TerrainElement terrainElement = new PathManager.TerrainElement(selection);

            //    if (route.property == SelectionManager.Property.Enter)
            //        path = terrainElement.Enter();

            //    break;

            case Enums.DataType.TerrainObject:

                PathManager.TerrainObject terrainObject = new PathManager.TerrainObject(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = terrainObject.Enter();

                break;

            case Enums.DataType.Item:

                PathManager.Item item = new PathManager.Item(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = item.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = item.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Get)
                    path = item.Get();

                break;

            case Enums.DataType.Element:

                PathManager.Element element = new PathManager.Element(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = element.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = element.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Get)
                    path = element.Get();

                break;

            case Enums.DataType.ObjectGraphic:

                PathManager.ObjectGraphic objectGraphic = new PathManager.ObjectGraphic(selection);

                if (selection.selectionProperty == SelectionManager.Property.Get)
                    path = objectGraphic.Get();

                break;

            case Enums.DataType.Option:

                PathManager.Option option = new PathManager.Option(selection);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = option.Enter();

            break;

            default: break;
        }
    }
}
