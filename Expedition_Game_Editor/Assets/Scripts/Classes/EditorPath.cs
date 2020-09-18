using UnityEngine;
using System.Linq;

public class EditorPath
{
    public Path path;

    public EditorPath(EditorElement editorElement, Route route)
    {
        if (editorElement.selectionProperty == SelectionManager.Property.Set) return;

        if (editorElement.selectionProperty == SelectionManager.Property.Toggle) return;

        if (editorElement.selectionProperty == SelectionManager.Property.Get)
        {
            PathManager.Search search = new PathManager.Search(editorElement, route);

            path = search.Get();

            return;
        }

        Debug.Log(editorElement.selectionProperty + " " + editorElement.DataElement.ElementData.DataType);

        switch (editorElement.DataElement.ElementData.DataType)
        {
            case Enums.DataType.Chapter:

                PathManager.Structure chapter = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = chapter.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = chapter.Edit();

                break;

            case Enums.DataType.Phase:

                PathManager.Structure phase = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = phase.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = phase.Edit();

                break;

            case Enums.DataType.Quest:

                PathManager.Structure quest = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = quest.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = quest.Edit();

                break;

            case Enums.DataType.Objective:

                PathManager.Structure objective = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = objective.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = objective.Edit();

                break;

            case Enums.DataType.WorldInteractable:

                //Don't stick with "Enter/Edit/Etc" restrictions
                //Add Combine, Extend: somewhat global, so not like "WorldInteractableExtend"

                //Add another path to root, to directly open Interaction without going through the path
                //The way an asset-less Interaction opens the selection is kind of a happy accident, but something more intentional would be preferable

                PathManager.WorldInteractable worldInteractable = new PathManager.WorldInteractable(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = worldInteractable.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Open)
                    path = worldInteractable.Open();

                if (editorElement.selectionProperty == SelectionManager.Property.OpenPhaseSaveRegionWorldInteractable)
                    path = worldInteractable.OpenPhaseSaveRegionWorldInteractable();

                break;

            case Enums.DataType.Task:

                PathManager.Structure task = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = task.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = task.Edit();

                if (editorElement.selectionProperty == SelectionManager.Property.Open)
                    path = task.Open();

                break;

            case Enums.DataType.Interaction:

                PathManager.Structure interaction = new PathManager.Structure(editorElement, route);
                
                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = interaction.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = interaction.Edit();

                if (editorElement.selectionProperty == SelectionManager.Property.Open)
                    path = interaction.Open();

                break;

            case Enums.DataType.InteractionDestination:

                PathManager.InteractionDestination interactionDestination = new PathManager.InteractionDestination(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = interactionDestination.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Open)
                    path = interactionDestination.Open();

                break;

            case Enums.DataType.Outcome:

                PathManager.Structure outcome = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = outcome.Enter();

                break;

            case Enums.DataType.Region:

                PathManager.Region region = new PathManager.Region(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = region.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = region.Edit();

                if (editorElement.selectionProperty == SelectionManager.Property.Open)
                    path = region.Open();

                if (editorElement.selectionProperty == SelectionManager.Property.OpenPhaseSaveRegion)
                    path = region.OpenPhaseSaveRegion();

                break;

            case Enums.DataType.Atmosphere:

                PathManager.Atmosphere atmosphere = new PathManager.Atmosphere(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = atmosphere.Enter();

                break;

            case Enums.DataType.Terrain:

                PathManager.Terrain terrain = new PathManager.Terrain(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = terrain.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = terrain.Edit();

                break;

            case Enums.DataType.WorldObject:

                PathManager.WorldObject worldObject = new PathManager.WorldObject(editorElement, route);
                
                if (editorElement.selectionProperty == SelectionManager.Property.Open)
                    path = worldObject.Open();

                break;

            case Enums.DataType.Item:

                PathManager.Item item = new PathManager.Item(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = item.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = item.Edit();

                break;

            case Enums.DataType.Interactable:

                PathManager.Interactable interactable = new PathManager.Interactable(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = interactable.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = interactable.Edit();

                break;

            case Enums.DataType.Option:

                PathManager.Option option = new PathManager.Option(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = option.Enter();

            break;

            case Enums.DataType.Save:

                PathManager.Save save = new PathManager.Save(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = save.EnterGame();

                break;

            case Enums.DataType.InteractableSave:

                PathManager.InteractableSave interactableSave = new PathManager.InteractableSave(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = interactableSave.Edit();

                break;

            case Enums.DataType.ChapterSave:

                PathManager.Structure chapterSave = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = chapterSave.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = chapterSave.Edit();

                break;

            case Enums.DataType.PhaseSave:

                PathManager.Structure phaseSave = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = phaseSave.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = phaseSave.Edit();

                break;

            case Enums.DataType.QuestSave:

                PathManager.Structure questSave = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = questSave.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = questSave.Edit();

                break;

            case Enums.DataType.ObjectiveSave:

                PathManager.Structure objectiveSave = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = objectiveSave.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = objectiveSave.Edit();

                break;

            case Enums.DataType.TaskSave:

                PathManager.Structure taskSave = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = taskSave.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = taskSave.Edit();

                break;

            case Enums.DataType.InteractionSave:

                PathManager.Structure interactionSave = new PathManager.Structure(editorElement, route);

                if (editorElement.selectionProperty == SelectionManager.Property.Enter)
                    path = interactionSave.Enter();

                if (editorElement.selectionProperty == SelectionManager.Property.Edit)
                    path = interactionSave.Edit();

                break;

            default: Debug.Log("CASE MISSING: " + editorElement.DataElement.ElementData.DataType); break;
        }
    }
}
