using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }

    public Enums.DataType DataType              { get { return Enums.DataType.Icon; } }
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
            dataList = IconDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var iconData = (IconElementData)searchElement.ElementData;

        switch (resultData.DataType)
        {
            case Enums.DataType.Icon:

                var resultElementData = (IconElementData)resultData;

                iconData.Id = resultElementData.Id;
                iconData.Path = resultElementData.Path;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}