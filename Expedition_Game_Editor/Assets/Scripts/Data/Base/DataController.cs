using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        DataType = dataType;
    }

    public void InitializeController() { }

    public void SetData(DataElement searchElement, IElementData resultElementData)
    {
        switch(DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableData(searchElement, resultElementData); break;
            case Enums.DataType.WorldObject:        SetWorldObjectData(searchElement, resultElementData);       break;

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

        switch(DataType)
        {
            case Enums.DataType.WorldInteractable:  Data.dataList = WorldInteractableDataManager.GetData(searchProperties); break;
            case Enums.DataType.Interaction:        Data.dataList = InteractionDataManager.GetData(searchProperties);       break;
            case Enums.DataType.WorldObject:        Data.dataList = WorldObjectDataManager.GetData(searchProperties);       break;
        }

        DataManager.ReplaceRouteData(this);
    }

    public void SetWorldInteractableData(DataElement searchElement, IElementData resultElementData)
    {
        var worldInteractableData = (WorldInteractableElementData)searchElement.ElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Interactable:

                var interactableData = (InteractableElementData)resultElementData;

                worldInteractableData.InteractableId = interactableData.Id;

                worldInteractableData.ModelId = interactableData.ModelId;

                worldInteractableData.ModelPath = interactableData.ModelPath;

                worldInteractableData.InteractableName = interactableData.Name;
                worldInteractableData.ModelIconPath = interactableData.ModelIconPath;
                worldInteractableData.ModelPath = interactableData.ModelPath;

                break;
        }
    }

    public void SetWorldObjectData(DataElement searchElement, IElementData resultElementData)
    {
        var worldObjectData = (WorldObjectElementData)searchElement.ElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Model:

                var modelData = (ModelElementData)resultElementData;

                worldObjectData.ModelId = modelData.Id;

                worldObjectData.ModelPath = modelData.Path;

                worldObjectData.ModelName = modelData.Name;
                worldObjectData.ModelIconPath = modelData.IconPath;
                
                worldObjectData.Height = modelData.Height;
                worldObjectData.Width = modelData.Width;
                worldObjectData.Depth = modelData.Depth;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
