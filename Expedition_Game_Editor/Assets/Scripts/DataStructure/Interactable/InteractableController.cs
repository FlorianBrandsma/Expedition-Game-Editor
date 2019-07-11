using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractableController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    private InteractableDataManager interactableDataManager       = new InteractableDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Interactable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interactable>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        interactableDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = interactableDataManager.GetInteractableDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (InteractableDataElement)searchElement.route.data.DataElement;

        var interactableDataElement = DataList.Cast<InteractableDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData.DataElement;

                interactableDataElement.id = resultElementData.id;
                interactableDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.route.data.DataElement = interactableDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
