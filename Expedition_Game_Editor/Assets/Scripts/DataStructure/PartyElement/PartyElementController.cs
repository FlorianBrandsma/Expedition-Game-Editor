using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyElementController : MonoBehaviour, IDataController
{
    public Search.Element searchParameters;

    public PartyElementDataManager partyElementDataManager = new PartyElementDataManager();

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType { get { return Enums.DataType.PartyElement; } }
    public List<IDataElement> DataList { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Element>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        partyElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = partyElementDataManager.GetPartyElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (PartyElementDataElement)searchElement.route.data.DataElement;

        var partyElementDataElement = DataList.Cast<PartyElementDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Element:

                var resultElementData = (ElementDataElement)resultData.DataElement;

                partyElementDataElement.ElementId = resultElementData.id;
                partyElementDataElement.objectGraphicIcon = resultElementData.objectGraphicIcon;

                break;
        }

        searchElement.route.data.DataElement = partyElementDataElement;
    }
}
