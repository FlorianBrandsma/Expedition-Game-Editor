using UnityEngine;
using System.Linq;

public class DataElement : MonoBehaviour
{
    public Data Data    { get; set; }
    public int Id       { get; set; }

    public IElementData ElementData { get { return Data.dataList.Where(x => x.Id == Id).FirstOrDefault(); } }

    public Path Path { get; set; }

    public SegmentController segmentController;
    public GameObject displayParent;
    
    public IDisplayManager DisplayManager { get; set; }

    public ISelectionElement SelectionElement { get { return GetComponent<ISelectionElement>(); } }
    public IPoolable Poolable   { get { return GetComponent<IPoolable>(); } }

    public void InitializeElement()
    {
        SelectionElement.InitializeElement();
    }

    public void InitializeElement(IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, bool uniqueSelection)
    {
        DisplayManager = displayManager;
        
        if(DisplayManager != null)
            segmentController = DisplayManager.Display.DataController.SegmentController;

        //Can be overwritten
        //data.dataController = DisplayManager.Display.DataController;

        SelectionElement.InitializeElement(selectionType, selectionProperty, uniqueSelection);
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
        
        //Replace search data with result data
        Data.dataController.SetData(ElementData, resultData);

        if(Data.dataController.SearchProperties != null)
        {
            if (Data.dataController.SearchProperties.autoUpdate)
            {
                ElementData.UpdateSearch();
            }  
        }

        //Apply combined search and result data
        segmentController.GetComponent<ISegment>().SetSearchResult(ElementData);
    }

    public void CancelDataSelection()
    {
        if (ElementData == null) return;

        ElementData.SelectionStatus = Enums.SelectionStatus.None;
    }
}
