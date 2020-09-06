using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }

    public Enums.DataType DataType              { get { return Enums.DataType.GameSave; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IElementData> DataList          { get; set; }

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
            dataList = GameSaveDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData) { }

    public void ToggleElement(EditorElement editorElement) { }
}
