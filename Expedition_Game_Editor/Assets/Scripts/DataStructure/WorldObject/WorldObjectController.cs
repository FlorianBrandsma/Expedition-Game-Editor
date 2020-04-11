using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.WorldObject; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectGraphic>().FirstOrDefault(); }
    }

    public WorldObjectController()
    {
        DataManager = new WorldObjectDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var worldObjectData = (WorldObjectDataElement)searchElement.data.dataElement;
        
        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData;

                worldObjectData.ObjectGraphicId = resultElementData.Id;
                worldObjectData.objectGraphicName = resultElementData.Name;
                worldObjectData.objectGraphicIconPath = resultElementData.iconPath;
                worldObjectData.objectGraphicPath = resultElementData.Path;

                worldObjectData.height = resultElementData.Height;
                worldObjectData.width = resultElementData.Width;
                worldObjectData.depth = resultElementData.Depth;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}