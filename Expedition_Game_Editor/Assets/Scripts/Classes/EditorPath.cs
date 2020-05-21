using UnityEngine;
using System.Linq;

public class EditorPath
{
    public Path path;

    public EditorPath(SelectionElement selection, Route route)
    {
        if (selection.selectionProperty == SelectionManager.Property.Set) return;

        if (selection.selectionProperty == SelectionManager.Property.Toggle) return;

        if (selection.selectionProperty == SelectionManager.Property.Get)
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

            case Enums.DataType.WorldInteractable:

                //Don't stick with "Enter/Edit/Etc" restrictions
                //Add Combine, Extend: somewhat global, so not like "WorldInteractableExtend"

                //Add another path to root, to directly open Interaction without going through the path
                //The way an asset-less Interaction opens the selection is kind of a happy accident, but something more intentional would be preferable

                PathManager.WorldInteractable worldInteractable = new PathManager.WorldInteractable(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = worldInteractable.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = worldInteractable.Open();

                if (selection.selectionProperty == SelectionManager.Property.OpenPhaseSaveRegionWorldInteractable)
                    path = worldInteractable.OpenPhaseSaveRegionWorldInteractable();

                break;

            case Enums.DataType.Task:

                PathManager.Structure task = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = task.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = task.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = task.Open();

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

            case Enums.DataType.Outcome:

                PathManager.Structure outcome = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = outcome.Enter();

                break;

            case Enums.DataType.Region:

                PathManager.Region region = new PathManager.Region(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = region.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = region.Edit();

                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = region.Open();

                if (selection.selectionProperty == SelectionManager.Property.OpenPhaseSaveRegion)
                    path = region.OpenPhaseSaveRegion();

                break;

            case Enums.DataType.Atmosphere:

                PathManager.Atmosphere atmosphere = new PathManager.Atmosphere(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = atmosphere.Enter();

                break;

            case Enums.DataType.Terrain:

                PathManager.Terrain terrain = new PathManager.Terrain(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = terrain.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            case Enums.DataType.WorldObject:

                PathManager.WorldObject worldObject = new PathManager.WorldObject(selection, route);
                
                if (selection.selectionProperty == SelectionManager.Property.Open)
                    path = worldObject.Open();

                break;

            case Enums.DataType.Item:

                PathManager.Item item = new PathManager.Item(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = item.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = item.Edit();

                break;

            case Enums.DataType.Interactable:

                PathManager.Interactable interactable = new PathManager.Interactable(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = interactable.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = interactable.Edit();

                if (selection.selectionProperty == SelectionManager.Property.OpenDataCharacters)
                    path = interactable.OpenDataCharacters();

                break;

            case Enums.DataType.Option:

                PathManager.Option option = new PathManager.Option(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = option.Enter();

            break;

            case Enums.DataType.Save:

                PathManager.Save save = new PathManager.Save(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = save.EnterGame();

                break;

            case Enums.DataType.ChapterSave:

                PathManager.Structure chapterSave = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = chapterSave.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = chapterSave.Edit();

                //if (selection.selectionProperty == SelectionManager.Property.Open)
                //    path = chapterSave.Open();

                break;

            case Enums.DataType.PhaseSave:

                PathManager.Structure phaseSave = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = phaseSave.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = phaseSave.Edit();

                //if (selection.selectionProperty == SelectionManager.Property.Open)
                //    path = chapterSave.Open();

                break;

            case Enums.DataType.QuestSave:

                PathManager.Structure questSave = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = questSave.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = questSave.Edit();

                //if (selection.selectionProperty == SelectionManager.Property.Open)
                //    path = chapterSave.Open();

                break;

            case Enums.DataType.ObjectiveSave:

                PathManager.Structure objectiveSave = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = objectiveSave.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = objectiveSave.Edit();

                //if (selection.selectionProperty == SelectionManager.Property.Open)
                //    path = chapterSave.Open();

                break;

            case Enums.DataType.TaskSave:

                PathManager.Structure taskSave = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = taskSave.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = taskSave.Edit();

                //if (selection.selectionProperty == SelectionManager.Property.Open)
                //    path = chapterSave.Open();

                break;

            case Enums.DataType.InteractionSave:

                PathManager.Structure interactionSave = new PathManager.Structure(selection, route);

                if (selection.selectionProperty == SelectionManager.Property.Enter)
                    path = interactionSave.Enter();

                if (selection.selectionProperty == SelectionManager.Property.Edit)
                    path = interactionSave.Edit();

                //if (selection.selectionProperty == SelectionManager.Property.Open)
                //    path = chapterSave.Open();

                break;

            default: Debug.Log("CASE MISSING: " + selection.GeneralData.DataType); break;
        }
    }
}
