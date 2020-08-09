using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SearchProperties
{
    public Enums.DataType dataType;
    public Enums.ElementType elementType;
    public Enums.IconType iconType;
    public bool autoUpdate;
    
    public IEnumerable searchParameters;
    
    public SearchProperties(Enums.DataType dataType)
    {
        this.dataType = dataType;

        Initialize();
    }

    public void Initialize()
    {
        if (searchParameters != null) return;

        switch (dataType)
        {
            case Enums.DataType.None: return;
            case Enums.DataType.Icon:                   searchParameters = new[] { new Search.Icon() };                     break;
            case Enums.DataType.ObjectGraphic:          searchParameters = new[] { new Search.ObjectGraphic() };            break;
            case Enums.DataType.Item:                   searchParameters = new[] { new Search.Item() };                     break;
            case Enums.DataType.Interactable:           searchParameters = new[] { new Search.Interactable() };             break;
            case Enums.DataType.Region:                 searchParameters = new[] { new Search.Region() };                   break;
            case Enums.DataType.Atmosphere:             searchParameters = new[] { new Search.Atmosphere() };               break;
            case Enums.DataType.Terrain:                searchParameters = new[] { new Search.Terrain() };                  break;
            case Enums.DataType.Tile:                   searchParameters = new[] { new Search.Tile() };                     break;
            case Enums.DataType.TerrainTile:            searchParameters = new[] { new Search.TerrainTile() };              break;
            case Enums.DataType.WorldObject:            searchParameters = new[] { new Search.WorldObject() };              break;
            case Enums.DataType.Chapter:                searchParameters = new[] { new Search.Chapter() };                  break;
            case Enums.DataType.PartyMember:            searchParameters = new[] { new Search.PartyMember() };              break;
            case Enums.DataType.ChapterInteractable:    searchParameters = new[] { new Search.ChapterInteractable() };      break;
            case Enums.DataType.ChapterRegion:          searchParameters = new[] { new Search.ChapterRegion() };            break;
            case Enums.DataType.Phase:                  searchParameters = new[] { new Search.Phase() };                    break;
            case Enums.DataType.Quest:                  searchParameters = new[] { new Search.Quest() };                    break;
            case Enums.DataType.Objective:              searchParameters = new[] { new Search.Objective() };                break;
            case Enums.DataType.WorldInteractable:      searchParameters = new[] { new Search.WorldInteractable() };        break;
            case Enums.DataType.Task:                   searchParameters = new[] { new Search.Task() };                     break;
            case Enums.DataType.Interaction:            searchParameters = new[] { new Search.Interaction() };              break;
            case Enums.DataType.InteractionDestination: searchParameters = new[] { new Search.InteractionDestination() };   break;
            case Enums.DataType.Outcome:                searchParameters = new[] { new Search.Outcome() };                  break;
            case Enums.DataType.EditorWorld:            searchParameters = new[] { new Search.EditorWorld() };              break;

            case Enums.DataType.GameWorld:              searchParameters = new[] { new Search.GameWorld() };                break;

            case Enums.DataType.Save:                   searchParameters = new[] { new Search.Save() };                     break;
            case Enums.DataType.GameSave:               searchParameters = new[] { new Search.GameSave() };                 break;
            case Enums.DataType.InteractableSave:       searchParameters = new[] { new Search.InteractableSave() };         break;
            case Enums.DataType.ChapterSave:            searchParameters = new[] { new Search.ChapterSave() };              break;
            case Enums.DataType.PhaseSave:              searchParameters = new[] { new Search.PhaseSave() };                break;
            case Enums.DataType.QuestSave:              searchParameters = new[] { new Search.QuestSave() };                break;
            case Enums.DataType.ObjectiveSave:          searchParameters = new[] { new Search.ObjectiveSave() };            break;
            case Enums.DataType.TaskSave:               searchParameters = new[] { new Search.TaskSave() };                 break;
            case Enums.DataType.InteractionSave:        searchParameters = new[] { new Search.InteractionSave() };          break;

            default: Debug.Log("CASE MISSING: " + dataType); break;
        }
    }
}
