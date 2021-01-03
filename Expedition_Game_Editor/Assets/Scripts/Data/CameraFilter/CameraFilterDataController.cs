using UnityEngine;
using System.Linq;

public class CameraFilterDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.CameraFilter; } }
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
            dataList = CameraFilterDataManager.GetData(searchProperties.searchParameters.Cast<Search.CameraFilter>().First()),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchCameraFilterElementData = (CameraFilterElementData)searchElementData;

        searchCameraFilterElementData.DataElement.Id = resultElementData.Id;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.CameraFilter:

                var resultCameraFilterElementData = (CameraFilterElementData)resultElementData;

                searchCameraFilterElementData.Id        = resultCameraFilterElementData.Id;

                searchCameraFilterElementData.Path      = resultCameraFilterElementData.Path;
                searchCameraFilterElementData.Name      = resultCameraFilterElementData.Name;

                searchCameraFilterElementData.IconPath  = resultCameraFilterElementData.IconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}