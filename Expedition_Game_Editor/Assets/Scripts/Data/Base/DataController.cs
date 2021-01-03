using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DataController : IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get; set; }

    public Data Data                            { get; set; }  
    public Enums.DataType DataType              { get; set; }
    public Enums.DataCategory DataCategory      { get; set; }

    public SearchProperties SearchProperties    { get; set; }

    public DataController(Enums.DataType dataType)
    {
        Data = new Data();
        Data.dataController = this;
        Data.dataList = new List<IElementData>();

        DataType = dataType;
    }

    public void InitializeController() { }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        switch(DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableData(searchElementData, resultElementData); break;
            case Enums.DataType.WorldObject:        SetWorldObjectData(searchElementData, resultElementData);       break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this
        };

        switch (DataType)
        {
            case Enums.DataType.WorldInteractable:
                Data.dataList = WorldInteractableDataManager.GetData(searchProperties.searchParameters.Cast<Search.WorldInteractable>().First());
                break;

            case Enums.DataType.Interaction:
                Data.dataList = InteractionDataManager.GetData(searchProperties.searchParameters.Cast<Search.Interaction>().First());
                break;

            case Enums.DataType.WorldObject:
                Data.dataList = WorldObjectDataManager.GetData(searchProperties.searchParameters.Cast<Search.WorldObject>().First());
                break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }

        Data.searchProperties = this.searchProperties;

        DataManager.ReplaceRouteData(this);
    }

    public void SetWorldInteractableData(IElementData searchElementData, IElementData resultElementData)
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
                searchWorldInteractableElementData.ModelPath        = resultInteractableElementData.ModelPath;

                break;
        }
    }

    public void SetWorldObjectData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchWorldObjectElementData = (WorldObjectElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Model:

                var resultModelElementData = (ModelElementData)resultElementData;

                searchWorldObjectElementData.ModelId        = resultModelElementData.Id;

                searchWorldObjectElementData.ModelPath      = resultModelElementData.Path;

                searchWorldObjectElementData.ModelName      = resultModelElementData.Name;
                searchWorldObjectElementData.ModelIconPath  = resultModelElementData.IconPath;
                
                searchWorldObjectElementData.Height         = resultModelElementData.Height;
                searchWorldObjectElementData.Width          = resultModelElementData.Width;
                searchWorldObjectElementData.Depth          = resultModelElementData.Depth;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
