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
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
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

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return objectGraphicDataManager.GetObjectGraphicDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData)
    {
        var searchElementData = (ObjectGraphicDataElement)searchElement.data.dataElement;

        var objectGraphicDataElement = DataList.Cast<ObjectGraphicDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.dataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData.dataElement;

                objectGraphicDataElement.id = resultElementData.id;
                objectGraphicDataElement.IconId = resultElementData.IconId;
                objectGraphicDataElement.Path = resultElementData.Path;
                objectGraphicDataElement.Name = resultElementData.Name;
                objectGraphicDataElement.iconPath = resultElementData.iconPath;
             
                break;
        }

        searchElement.data.dataElement = objectGraphicDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
