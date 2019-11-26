using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericDataController : IDataController
{
    public IDataManager dataManager;

    private List<IDataElement> dataList = new List<IDataElement>();

    public SegmentController SegmentController { get; set; }
    
    public List<IDataElement> DataList
    {
        get { return dataList; }
        set { dataList = value; }
    }

    public Enums.DataType DataType { get; set; }

    public Enums.DataCategory DataCategory { get; set; }

    public IEnumerable SearchParameters { get; set; }

    public GenericDataController(Enums.DataType dataType)
    {
        DataType = dataType;

        switch(DataType)
        {
            case Enums.DataType.SceneInteractable:  dataManager = new SceneInteractableDataManager(this);   break;
            case Enums.DataType.Interaction:        dataManager = new InteractionDataManager(this);         break;
            case Enums.DataType.SceneObject:        dataManager = new SceneObjectDataManager(this);         break;
        }
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return dataManager.GetDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultDataElement) { }

    public void ToggleElement(IDataElement dataElement) { }
}
