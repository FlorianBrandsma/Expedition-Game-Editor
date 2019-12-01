using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ObjectGraphic; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectGraphic>().FirstOrDefault(); }
    }

    public ObjectGraphicController()
    {
        DataManager = new ObjectGraphicDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var objectGraphicData = (ObjectGraphicDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData;

                objectGraphicData.Id = resultElementData.Id;
                objectGraphicData.IconId = resultElementData.IconId;
                objectGraphicData.Path = resultElementData.Path;
                objectGraphicData.Name = resultElementData.Name;
                objectGraphicData.iconPath = resultElementData.iconPath;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
