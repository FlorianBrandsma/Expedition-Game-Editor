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

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return partyMemberDataManager.GetPartyMemberDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData)
    {
        var searchElementData = (PartyMemberDataElement)searchElement.data.dataElement;

        var partyMemberDataElement = DataList.Cast<PartyMemberDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.dataController.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData.dataElement;

                partyMemberDataElement.InteractableId = resultElementData.id;
                partyMemberDataElement.interactableName = resultElementData.Name;
                partyMemberDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.data.dataElement = partyMemberDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
