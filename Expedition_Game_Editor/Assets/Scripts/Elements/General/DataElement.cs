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

    public void SetSearchResult(DataRequest dataRequest, IElementData resultElementData)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        var searchElementData = ElementData;

        if (searchElementData.Id == 0 && resultElementData.Id == 0) return;

        //Element belongs to a list if display manager is not null.
        //If the id is also zero, a new element should be added to the list.
        //Clearer solution would be to include an "add/replace" option.
        if (DisplayManager != null && searchElementData.Id == 0)
            searchElementData = ElementData.Clone();

        Data.dataController.SetData(searchElementData, resultElementData);
        
        if (Data.dataController.SearchProperties != null && Data.dataController.SearchProperties.autoUpdate)
        {
            dataRequest.requestType = Enums.RequestType.Execute;
            ApplyChanges(dataRequest, searchElementData, resultElementData);
        }

        //Apply combined search and result data
        segmentController.GetComponent<ISegment>().SetSearchResult(searchElementData, resultElementData);
    }

    private static void ApplyChanges(DataRequest dataRequest, IElementData searchElementData, IElementData resultElementData)
    {
        if (resultElementData.Id > 0)
        {
            if (searchElementData.ExecuteType == Enums.ExecuteType.Add)
                searchElementData.Add(dataRequest);

            if (searchElementData.ExecuteType == Enums.ExecuteType.Update)
                searchElementData.Update(dataRequest);

        } else {

            searchElementData.Remove(dataRequest);
        }
    }
    
    public void CancelDataSelection()
    {
        if (ElementData == null) return;

        ElementData.SelectionStatus = Enums.SelectionStatus.None;
    }
}
