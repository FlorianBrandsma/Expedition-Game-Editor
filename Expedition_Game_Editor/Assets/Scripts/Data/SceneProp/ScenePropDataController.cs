using UnityEngine;

public class ScenePropDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.SceneProp; } }
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
            dataList = ScenePropDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchScenePropElementData = (ScenePropElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Model:

                var resultModelElementData = (ModelElementData)resultElementData;

                searchScenePropElementData.ModelId = resultModelElementData.Id;

                searchScenePropElementData.ModelName = resultModelElementData.Name;
                searchScenePropElementData.ModelIconPath = resultModelElementData.IconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}