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
    
    public IDisplayManager DisplayManager       { get; set; }

    public ISelectionElement SelectionElement   { get { return GetComponent<ISelectionElement>(); } }
    public IPoolable Poolable                   { get { return GetComponent<IPoolable>(); } }

    public void InitializeElement()
    {
        SelectionElement.InitializeElement();
    }

    public void InitializeElement(IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty, SelectionManager.Property addProperty, bool uniqueSelection)
    {
        DisplayManager = displayManager;
        
        if(DisplayManager != null)
            segmentController = DisplayManager.Display.DataController.SegmentController;

        SelectionElement.InitializeElement(selectionType, selectionProperty, addProperty, uniqueSelection);
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

        var searchElementData = ElementData;

        if (searchElementData.Id == 0 && resultData.Id == 0) return;

        //When adding a new element by searching, should it overwrite the original
        //and insert a new default element, or return a new element? 
        //In case of a new element, the segment can decide what to do with it

        //Element belongs to a list if display manager is not null.
        //If the id is also zero, a new element should be added to the list.
        //Clearer solution would be to include an "add/replace" option.
        if (DisplayManager != null && searchElementData.Id == 0)
        {
            searchElementData = ElementData.Clone();
        }
        
        //What happens when auto update is true? Or when selecting a result which
        //remove the selected row?
        
        Data.dataController.SetData(searchElementData, resultData);
        
        if(Data.dataController.SearchProperties != null)
        {
            if (Data.dataController.SearchProperties.autoUpdate)
            {
                if(resultData.Id > 0)
                {
                    searchElementData.UpdateSearch();

                } else {

                    Debug.Log("Automatically delete " + searchElementData.DataType + "" + searchElementData.Id);
                }
            }  
        }

        //Apply combined search and result data
        segmentController.GetComponent<ISegment>().SetSearchResult(searchElementData, resultData);
    }

    public void CancelDataSelection()
    {
        if (ElementData == null) return;

        ElementData.SelectionStatus = Enums.SelectionStatus.None;
    }
}
