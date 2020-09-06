using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.WorldObject; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    
    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }
    
    public void InitializeController()
    {
        SearchProperties.Initialize();
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
            dataList = WorldObjectDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var worldObjectData = (WorldObjectElementData)searchElement.ElementData;
        
        switch (resultData.DataType)
        {
            case Enums.DataType.Model:

                var resultElementData = (ModelElementData)resultData;

                worldObjectData.ModelId = resultElementData.Id;

                worldObjectData.ModelPath = resultElementData.Path;

                worldObjectData.ModelName = resultElementData.Name;
                worldObjectData.ModelIconPath = resultElementData.IconPath;
                
                worldObjectData.Height = resultElementData.Height;
                worldObjectData.Width = resultElementData.Width;
                worldObjectData.Depth = resultElementData.Depth;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}