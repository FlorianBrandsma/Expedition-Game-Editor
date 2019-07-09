using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementController : MonoBehaviour, IDataController
{
    public Search.Element searchParameters;

    private ElementDataManager elementDataManager       = new ElementDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Element; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Element>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        elementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = elementDataManager.GetElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (ElementDataElement)searchElement.route.data.DataElement;

        var elementDataElement = DataList.Cast<ElementDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Element:

                var resultElementData = (ElementDataElement)resultData.DataElement;

                elementDataElement.id = resultElementData.id;
                elementDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.route.data.DataElement = elementDataElement;
    }

    public void ToggleElement(IDataElement dataElement)
    {

    }
}
