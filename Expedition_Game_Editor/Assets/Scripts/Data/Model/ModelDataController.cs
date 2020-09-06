using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ModelDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.Model; } }
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
            dataList = ModelDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var modelData = (ModelElementData)searchElement.ElementData;

        searchElement.Id = resultData.Id;

        switch (resultData.DataType)
        {
            case Enums.DataType.Model:

                var resultElementData = (ModelElementData)resultData;

                modelData.Id = resultElementData.Id;
                modelData.IconId = resultElementData.IconId;
                modelData.Path = resultElementData.Path;
                modelData.Name = resultElementData.Name;

                modelData.Height = resultElementData.Height;
                modelData.Width = resultElementData.Width;
                modelData.Depth = resultElementData.Depth;

                modelData.IconPath = resultElementData.IconPath;
                
                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
