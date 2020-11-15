using UnityEngine;

public class WorldObjectDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.WorldObject; } }
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
            dataList = WorldObjectDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchWorldObjectElementData = (WorldObjectElementData)searchElementData;
        
        switch (resultElementData.DataType)
        {
            case Enums.DataType.Model:

                var resultModelElementData = (ModelElementData)resultElementData;

                searchWorldObjectElementData.ModelId        = resultModelElementData.Id;

                searchWorldObjectElementData.ModelPath      = resultModelElementData.Path;

                searchWorldObjectElementData.ModelName      = resultModelElementData.Name;
                searchWorldObjectElementData.ModelIconPath  = resultModelElementData.IconPath;
                
                searchWorldObjectElementData.Height         = resultModelElementData.Height;
                searchWorldObjectElementData.Width          = resultModelElementData.Width;
                searchWorldObjectElementData.Depth          = resultModelElementData.Depth;

                break;

            default: Debug.Log("CASE MISSING: " + resultElementData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}