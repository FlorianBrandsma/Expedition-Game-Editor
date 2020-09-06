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
            dataList = PartyMemberDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var partyMemberData = (PartyMemberElementData)searchElement.ElementData;

        switch (resultData.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableElementData)resultData;

                partyMemberData.InteractableId = resultElementData.Id;
                partyMemberData.InteractableName = resultElementData.Name;
                partyMemberData.ModelIconPath = resultElementData.ModelIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
