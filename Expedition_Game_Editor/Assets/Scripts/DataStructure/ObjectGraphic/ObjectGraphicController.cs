using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    private ObjectGraphicDataManager objectGraphicDataManager = new ObjectGraphicDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ObjectGraphic; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectGraphic>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        objectGraphicDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = objectGraphicDataManager.GetObjectGraphicDataElements(searchParameters);

        var objectGraphicDataElements = DataList.Cast<ObjectGraphicDataElement>();
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (ObjectGraphicDataElement)searchElement.route.data.DataElement;

        var objectGraphicDataElement = DataList.Cast<ObjectGraphicDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData.DataElement;

                objectGraphicDataElement.id = resultElementData.id;
                objectGraphicDataElement.Icon = resultElementData.Icon;
                objectGraphicDataElement.Path = resultElementData.Path;
                objectGraphicDataElement.Name = resultElementData.Name;
             
                break;
        }

        searchElement.route.data.DataElement = objectGraphicDataElement;
    }
}
