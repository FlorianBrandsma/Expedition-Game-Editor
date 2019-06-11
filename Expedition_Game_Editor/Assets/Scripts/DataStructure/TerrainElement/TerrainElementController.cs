using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementController : MonoBehaviour, IDataController
{
    public Search.Element searchParameters;

    public TerrainElementDataManager terrainElementDataManager = new TerrainElementDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainElement; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Element>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        terrainElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = terrainElementDataManager.GetTerrainElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (TerrainElementDataElement)searchElement.route.data.DataElement;

        var terrainElementDataElement = DataList.Cast<TerrainElementDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Element:

                var resultElementData = (ElementDataElement)resultData.DataElement;

                terrainElementDataElement.ElementId = resultElementData.id;
                terrainElementDataElement.elementName = resultElementData.Name;
                terrainElementDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.route.data.DataElement = terrainElementDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
