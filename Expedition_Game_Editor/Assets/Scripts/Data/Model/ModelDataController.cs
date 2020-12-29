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
            dataList = ModelDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchModelElementData = (ModelElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Model:

                var resultModelElementData = (ModelElementData)resultElementData;
                
                searchModelElementData.Id       = resultModelElementData.Id;
                searchModelElementData.IconId   = resultModelElementData.IconId;
                searchModelElementData.Path     = resultModelElementData.Path;
                searchModelElementData.Name     = resultModelElementData.Name;

                searchModelElementData.Height   = resultModelElementData.Height;
                searchModelElementData.Width    = resultModelElementData.Width;
                searchModelElementData.Depth    = resultModelElementData.Depth;

                searchModelElementData.IconPath = resultModelElementData.IconPath;
                
                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
