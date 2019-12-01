using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconController : MonoBehaviour, IDataController
{
    public Search.Icon searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Icon; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Icon>().FirstOrDefault(); }
    }

    public IconController()
    {
        DataManager = new IconDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var iconData = (IconDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Icon:

                var resultElementData = (IconDataElement)resultData;

                iconData.Id = resultElementData.Id;
                iconData.Path = resultElementData.Path;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}