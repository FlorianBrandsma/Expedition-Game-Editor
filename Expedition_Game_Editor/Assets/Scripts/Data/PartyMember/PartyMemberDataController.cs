using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyMemberDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.PartyMember; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    
    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }
    
    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this,
            dataList = PartyMemberDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchPartyMemberElementData = (PartyMemberElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Interactable:

                var resultInteractableElementData = (InteractableElementData)resultElementData;

                searchPartyMemberElementData.InteractableId     = resultInteractableElementData.Id;
                searchPartyMemberElementData.InteractableName   = resultInteractableElementData.Name;
                searchPartyMemberElementData.ModelIconPath      = resultInteractableElementData.ModelIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
