using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyMemberController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    public PartyMemberDataManager partyMemberDataManager = new PartyMemberDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.PartyMember; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interactable>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        partyMemberDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = partyMemberDataManager.GetPartyMemberDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (PartyMemberDataElement)searchElement.route.data.DataElement;

        var partyMemberDataElement = DataList.Cast<PartyMemberDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData.DataElement;

                partyMemberDataElement.InteractableId = resultElementData.id;
                partyMemberDataElement.interactableName = resultElementData.Name;
                partyMemberDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.route.data.DataElement = partyMemberDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
