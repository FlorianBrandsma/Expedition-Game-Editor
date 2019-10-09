using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyMemberController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    public PartyMemberDataManager partyMemberDataManager;

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

    public PartyMemberController()
    {
        partyMemberDataManager = new PartyMemberDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return partyMemberDataManager.GetPartyMemberDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var partyMemberData = (PartyMemberDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).dataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData;

                partyMemberData.InteractableId = resultElementData.Id;
                partyMemberData.interactableName = resultElementData.Name;
                partyMemberData.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
