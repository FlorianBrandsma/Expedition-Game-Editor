using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementController : MonoBehaviour, IDataController
{
    public Search.Element searchParameters;

    private ElementManager elementManager       = new ElementManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Element; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Element>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        elementManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = elementManager.GetElementDataElements(searchParameters);

        var elementDataElements = DataList.Cast<ElementDataElement>();

        //elementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //elementDataElements[0].Update();
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = searchElement.route.data.ElementData.Cast<ElementDataElement>().FirstOrDefault();

        var elementDataElement = DataList.Cast<ElementDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Element:

                var resultElementData = resultData.ElementData.Cast<ElementDataElement>().FirstOrDefault();

                elementDataElement.id = resultElementData.id;
                elementDataElement.objectGraphicIcon = resultElementData.objectGraphicIcon;

                break;
        }

        searchElement.route.data.ElementData = new[] { elementDataElement };
    }
}
