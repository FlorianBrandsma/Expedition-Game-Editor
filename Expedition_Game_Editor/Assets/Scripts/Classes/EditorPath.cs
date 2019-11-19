using UnityEngine;
using System.Linq;

public class EditorPath
{
    public Path path;

    public EditorPath(SelectionElement selection, Route route)
    {
        if(selection.selectionProperty == SelectionManager.Property.Get)
        {
            PathManager.Search search = new PathManager.Search(selection, route);

            path = search.Get();

            return;
        }

        switch (selection.GeneralData.DataType)
        {
            case Enums.DataType.Chapter:

                PathManager.Structure chapter = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = chapter.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = chapter.Edit();

                break;

            case Enums.DataType.Phase:

                PathManager.Structure phase = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = phase.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = phase.Edit();

                break;

            case Enums.DataType.Quest:

                PathManager.Structure quest = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = quest.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = quest.Edit();

                break;

            case Enums.DataType.Objective:

                PathManager.Structure objective = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = objective.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = objective.Edit();

                break;

            case Enums.DataType.SceneInteractable:

                //Don't stick with "Enter/Edit/Etc" restrictions
                //Add Combine, Extend: somewhat global, so not like "SceneInteractableExtend"

                //Add another path to root, to directly open Interaction without going through the path
                //The way an asset-less Interaction opens the selection is kind of a happy accident, but something more intentional would be preferable

                PathManager.SceneInteractable sceneInteractable = new PathManager.SceneInteractable(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = sceneInteractable.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = sceneInteractable.Open();
                
                break;

            case Enums.DataType.Interaction:

                PathManager.Structure interaction = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = interaction.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = interaction.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = interaction.Open();

                break;

            case Enums.DataType.Region:

                PathManager.Region region = new PathManager.Region(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = region.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = region.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = region.Open();

                break;

            case Enums.DataType.Terrain:

                PathManager.Terrain terrain = new PathManager.Terrain(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            case Enums.DataType.SceneObject:

                PathManager.SceneObject sceneObject = new PathManager.SceneObject(selection, route);
                
                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = sceneObject.Open();

                break;

            case Enums.DataType.Item:

                PathManager.Item item = new PathManager.Item(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = item.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = item.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Get)
                    path = item.Get();

                break;

            case Enums.DataType.Interactable:

                PathManager.Interactable interactable = new PathManager.Interactable(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = interactable.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = interactable.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Get)
                    path = interactable.Get();

                break;

            case Enums.DataType.ObjectGraphic:

                PathManager.ObjectGraphic objectGraphic = new PathManager.ObjectGraphic(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Get)
                    path = objectGraphic.Get();

                break;

            case Enums.DataType.Option:

                PathManager.Option option = new PathManager.Option(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = option.Enter();

            break;

            default: break;
        }
    }
}
