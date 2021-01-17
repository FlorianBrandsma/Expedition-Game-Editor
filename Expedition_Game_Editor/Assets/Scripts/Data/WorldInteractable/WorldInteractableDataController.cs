using UnityEngine;
using System.Linq;

public class WorldInteractableDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;
    //public Search.Interactable searchInteractable;

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
        SearchProperties.Initialize();

        //Temporary until search parameters needs another rework

        //Search was reworked so that one controller could find
        //data from multiple data types, but lost the ability to provide standard
        //search parameters on a component basis. At the moment, no controller
        //uses multiple data types 1/5/2020

        //Update 20/12/2020: scene actors can look for other scene actors as well as world interactables!

        //SearchProperties.searchParameters = new[] { searchInteractable };
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
            dataList = WorldInteractableDataManager.GetData(searchProperties.searchParameters.Cast<Search.WorldInteractable>().First()),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchWorldInteractableElementData = (WorldInteractableElementData)searchElementData;
        
        switch (resultElementData.DataType)
        {
            case Enums.DataType.Interactable:

                var resultInteractableElementData = (InteractableElementData)resultElementData;

                searchWorldInteractableElementData.InteractableId   = resultInteractableElementData.Id;

                searchWorldInteractableElementData.ModelId          = resultInteractableElementData.ModelId;

                searchWorldInteractableElementData.ModelPath        = resultInteractableElementData.ModelPath;

                searchWorldInteractableElementData.InteractableName = resultInteractableElementData.Name;
                searchWorldInteractableElementData.ModelIconPath    = resultInteractableElementData.ModelIconPath;

                searchWorldInteractableElementData.Height           = resultInteractableElementData.Height;
                searchWorldInteractableElementData.Width            = resultInteractableElementData.Width;
                searchWorldInteractableElementData.Depth            = resultInteractableElementData.Depth;

                searchWorldInteractableElementData.Scale            = resultInteractableElementData.Scale;

                break;

            case Enums.DataType.WorldInteractable:

                var resultWorldInteractableElementData = (WorldInteractableElementData)resultElementData;

                searchWorldInteractableElementData.Id = resultWorldInteractableElementData.Id;

                searchWorldInteractableElementData.InteractableId   = resultWorldInteractableElementData.InteractableId;

                searchWorldInteractableElementData.ModelId          = resultWorldInteractableElementData.ModelId;

                searchWorldInteractableElementData.ModelPath        = resultWorldInteractableElementData.ModelPath;

                searchWorldInteractableElementData.InteractableName = resultWorldInteractableElementData.InteractableName;
                searchWorldInteractableElementData.ModelIconPath    = resultWorldInteractableElementData.ModelIconPath;

                searchWorldInteractableElementData.Height           = resultWorldInteractableElementData.Height;
                searchWorldInteractableElementData.Width            = resultWorldInteractableElementData.Width;
                searchWorldInteractableElementData.Depth            = resultWorldInteractableElementData.Depth;

                searchWorldInteractableElementData.Scale            = resultWorldInteractableElementData.Scale;

                break;

            default: Debug.Log("CASE MISSING: " + resultElementData.DataType); break;
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
