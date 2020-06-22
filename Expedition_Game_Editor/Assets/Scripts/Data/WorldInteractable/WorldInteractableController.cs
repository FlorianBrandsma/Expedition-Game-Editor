using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;
    public Search.Interactable searchInteractable;

    public IDataManager DataManager             { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.WorldInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public WorldInteractableController()
    {
        DataManager = new WorldInteractableDataManager(this);
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

    public void SetData(DataElement searchElement, IDataElement resultData)
    {
        var worldInteractableData = (WorldInteractableDataElement)searchElement.data.dataElement;

        switch (resultData.DataType)
        {
            case Enums.DataType.Interactable:

                var interactableData = (InteractableDataElement)resultData;

                worldInteractableData.InteractableId = interactableData.Id;

                worldInteractableData.objectGraphicId = interactableData.ObjectGraphicId;

                worldInteractableData.objectGraphicPath = interactableData.objectGraphicPath;

                worldInteractableData.interactableName = interactableData.Name;
                worldInteractableData.objectGraphicIconPath = interactableData.objectGraphicIconPath;

                break;

            default: Debug.Log("CASE MISSING: " + resultData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement)
    {
        var worldInteractablesData = (WorldInteractableDataElement)editorElement.DataElement.data.dataElement;
        var questData = (QuestDataElement)SegmentController.EditorController.PathController.route.data.dataElement;

        switch (worldInteractablesData.elementStatus)
        {
            case Enums.ElementStatus.Enabled:

                worldInteractablesData.QuestId = 0;
                worldInteractablesData.elementStatus = Enums.ElementStatus.Disabled;

                break;

            case Enums.ElementStatus.Disabled:

                worldInteractablesData.QuestId = questData.Id;
                worldInteractablesData.elementStatus = Enums.ElementStatus.Enabled;

                break;
        }

        SegmentController.Segment.DataEditor.UpdateEditor();

        editorElement.elementStatus = worldInteractablesData.elementStatus;
        editorElement.SetStatus();
    }
}
