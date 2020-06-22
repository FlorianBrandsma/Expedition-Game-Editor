using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DataElement : MonoBehaviour
{
    public class Data
    {
        public IDataController dataController;
        public IElementData elementData;
        public SearchProperties searchProperties;

        public Data() { }

        public Data(Route.Data data)
        {
            dataController = data.dataController;
            elementData = data.elementData;
        }

        public Data(IDataController dataController)
        {
            elementData = new GeneralElementData();

            if (dataController != null)
            {
                this.dataController = dataController;
                elementData.DataType = dataController.DataType;
            }
        }
        
        public Data(IDataController dataController, IElementData elementData)
        {
            this.dataController = dataController;
            this.elementData = elementData;
        }

        public Data(IDataController dataController, IElementData elementData, SearchProperties searchProperties)
        {
            this.dataController = dataController;
            this.elementData = elementData;
            this.searchProperties = searchProperties;
        }
    }

    public Data data = new Data();
    public Path Path { get; set; }

    public SegmentController segmentController;
    public GameObject displayParent;

    public GeneralData GeneralData { get { return (GeneralData)data.elementData; } }

    public IDisplayManager DisplayManager { get; set; }

    public ISelectionElement SelectionElement { get { return GetComponent<ISelectionElement>(); } }
    public IPoolable Poolable   { get { return GetComponent<IPoolable>(); } }
    public IElement Element     { get { return GetComponent<IElement>(); } }

    public void InitializeElement(IDataController dataController)
    {
        data = new Data(dataController);

        SelectionElement.InitializeElement();
    }

    public void InitializeElement(Route.Data data)
    {
        this.data = new Data(data);

        SelectionElement.InitializeElement();
    }

    public void InitializeElement(IDisplayManager displayManager, SelectionManager.Type selectionType, SelectionManager.Property selectionProperty)
    {
        DisplayManager = displayManager;

        segmentController = DisplayManager.Display.DataController.SegmentController;

        //Can be overwritten
        data.dataController = DisplayManager.Display.DataController;

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
            displayParent.GetComponent<IDisplay>().DataController = data.dataController;
    }

    public void SetResult(IElementData resultData)
    {
        if (displayParent != null)
            displayParent.GetComponent<IDisplay>().ClearDisplay();

        data.dataController.SetData(this, resultData);
        
        if(data.dataController.SearchProperties != null)
        {
            if (data.dataController.SearchProperties.autoUpdate)
                data.elementData.UpdateSearch();
        }
        
        segmentController.GetComponent<ISegment>().SetSearchResult(this);
    }
    
    public void CancelDataSelection()
    {
        if (data.elementData == null) return;

        data.elementData.SelectionStatus = Enums.SelectionStatus.None;
    }
}
