using UnityEngine;

public class IconDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.Icon; } }
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
            dataList = IconDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchIconElementData = (IconElementData)searchElementData;

        searchIconElementData.DataElement.Id = resultElementData.Id;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Icon:

                var resultIconElementData = (IconElementData)resultElementData;

                searchIconElementData.Id    = resultIconElementData.Id;
                searchIconElementData.Path  = resultIconElementData.Path;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}