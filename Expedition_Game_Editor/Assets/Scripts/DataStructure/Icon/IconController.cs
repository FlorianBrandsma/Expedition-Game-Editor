using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconController : MonoBehaviour, IDataController
{
    public Search.Icon searchParameters;

    private IconDataManager iconDataManager;

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
        iconDataManager = new IconDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return iconDataManager.GetIconDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var iconData = (IconDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).dataType)
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