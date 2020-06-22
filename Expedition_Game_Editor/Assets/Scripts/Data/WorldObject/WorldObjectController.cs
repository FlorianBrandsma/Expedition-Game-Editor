using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.WorldObject; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IElementData> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public WorldObjectController()
    {
        DataManager = new WorldObjectDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var worldObjectData = (WorldObjectElementData)searchElement.data.elementData;
        
        switch (resultData.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicElementData)resultData;

                worldObjectData.ObjectGraphicId = resultElementData.Id;

                worldObjectData.objectGraphicPath = resultElementData.Path;

                worldObjectData.objectGraphicName = resultElementData.Name;
                worldObjectData.objectGraphicIconPath = resultElementData.iconPath;
                
                worldObjectData.height = resultElementData.Height;
                worldObjectData.width = resultElementData.Width;
                worldObjectData.depth = resultElementData.Depth;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}