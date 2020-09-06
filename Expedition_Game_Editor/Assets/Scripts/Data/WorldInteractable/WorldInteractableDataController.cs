using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;
    public Search.Interactable searchInteractable;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.WorldInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    
    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }
    
    public void InitializeController()
    {
        //SearchProperties.Initialize();

        //Temporary until search parameters needs another rework

        //Search was reworked so that one controller could find
        //data from multiple data types, but lost the ability to provide standard
        //search parameters on a component basis. At the moment, no controller
        //uses multiple data types 1/5/2020

        SearchProperties.searchParameters = new[] { searchInteractable };
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this,
            dataList = WorldInteractableDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var worldInteractableData = (WorldInteractableElementData)searchElement.ElementData;

        switch (resultData.DataType)
        {
            case Enums.DataType.Interactable:

                var interactableData = (InteractableElementData)resultData;

                worldInteractableData.InteractableId = interactableData.Id;

                worldInteractableData.ModelId = interactableData.ModelId;

                worldInteractableData.ModelPath = interactableData.ModelPath;

                worldInteractableData.InteractableName = interactableData.Name;
                worldInteractableData.ModelIconPath = interactableData.ModelIconPath;

                break;

            default: Debug.Log("CASE MISSING: " + resultData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement)
    {
        var worldInteractableData = (WorldInteractableElementData)editorElement.DataElement.ElementData;
        var questData = (QuestElementData)SegmentController.EditorController.PathController.route.ElementData;

        switch (worldInteractableData.ElementStatus)
        {
            case Enums.ElementStatus.Enabled:

                worldInteractableData.QuestId = 0;
                worldInteractableData.ElementStatus = Enums.ElementStatus.Disabled;

                break;

            case Enums.ElementStatus.Disabled:

                worldInteractableData.QuestId = questData.Id;
                worldInteractableData.ElementStatus = Enums.ElementStatus.Enabled;

                break;
        }

        SegmentController.Segment.DataEditor.UpdateEditor();

        editorElement.elementStatus = worldInteractableData.ElementStatus;
        editorElement.SetStatus();
    }
}
