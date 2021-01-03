using UnityEngine;
using System.Linq;

public class GameSaveDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get; set; }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.GameSave; } }
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
            dataList = GameSaveDataManager.GetData(searchProperties.searchParameters.Cast<Search.GameSave>().First()),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData) { }

    public void ToggleElement(EditorElement editorElement) { }
}
