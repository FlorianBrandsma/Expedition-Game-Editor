using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconController : MonoBehaviour, IDataController
{
    public Search.Icon searchParameters;

    private IconDataManager iconDataManager = new IconDataManager();

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

    public void InitializeController()
    {
        iconDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = iconDataManager.GetIconDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (IconDataElement)searchElement.route.data.DataElement;

        var iconDataElement = DataList.Cast<IconDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Icon:

                var resultElementData = (IconDataElement)resultData.DataElement;

                iconDataElement.id = resultElementData.id;
                iconDataElement.Path = resultElementData.Path;

                break;
        }

        searchElement.route.data.DataElement = iconDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}