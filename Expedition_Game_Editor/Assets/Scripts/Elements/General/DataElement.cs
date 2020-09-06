using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class DataElement : MonoBehaviour
{
    public Data Data    { get; set; }
    public int Id       { get; set; }

    public IElementData ElementData { get { return Data.dataList.Where(x => x.Id == Id).First(); } }

    public Path Path { get; set; }

    public SegmentController segmentController;
    public GameObject displayParent;
    
    public IDisplayManager DisplayManager { get; set; }

    public ISelectionElement SelectionElement { get { return GetComponent<ISelectionElement>(); } }
    public IPoolable Poolable   { get { return GetComponent<IPoolable>(); } }
    public IElement Element     { get { return GetComponent<IElement>(); } }

    public void InitializeElement(Route route)
    {
        Id = route.id;

        InitializeElement(route.data.dataController);
    }

    public void InitializeElement()
    {
        SelectionElement.InitializeElement();
    }

    public void InitializeElement(IDataController dataController)
    {
        Data = dataController.Data;

        SelectionElement.InitializeElement();
    }

    public void InitializeElement(IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        DisplayManager = displayManager;

        segmentController = DisplayManager.Display.DataController.SegmentController;

        //Can be overwritten
        //data.dataController = DisplayManager.Display.DataController;

        SelectionElement.InitializeElement(selectionType, selectionProperty);
    }

    public void UpdateElement()
    {
        SelectionElement.UpdateElement();
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();

        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().DataController = Data.dataController;
    }

    public void SetResult(IElementData resultData)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        Data.dataController.SetData(this, resultData);
        
        if(Data.dataController.SearchProperties != null)
        {
            if (Data.dataController.SearchProperties.autoUpdate)
                ElementData.UpdateSearch();
        }
        
        segmentController.GetComponent<ISegment>().SetSearchResult(this);
    }

    public void CancelDataSelection()
    {
        if (ElementData == null) return;

        ElementData.SelectionStatus = Enums.SelectionStatus.None;
    }
}
