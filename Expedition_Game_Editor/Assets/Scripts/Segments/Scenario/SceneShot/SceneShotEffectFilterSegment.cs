using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneShotEffectFilterSegment : MonoBehaviour, ISegment
{
    public EditorElement cameraFilterButton;

    private CameraFilterDataController CameraFilterDataController   { get { return GetComponent<CameraFilterDataController>(); } }
    private CameraFilterElementData CameraFilterElementData         { get { return (CameraFilterElementData)cameraFilterButton.DataElement.ElementData; } }
    
    public SegmentController SegmentController                      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                                       { get; set; }

    private SceneShotEditor SceneShotEditor                         { get { return (SceneShotEditor)DataEditor; } }
    
    #region Data properties
    private int Id
    {
        get { return SceneShotEditor.Id; }
    }

    private int CameraFilterId
    {
        get { return SceneShotEditor.CameraFilterId; }
        set { SceneShotEditor.CameraFilterId = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeSceneButton();
    }

    private void InitializeSceneButton()
    {
        //Get camera filter data
        var searchProperties = new SearchProperties(Enums.DataType.CameraFilter);
        var searchParameters = searchProperties.searchParameters.Cast<Search.CameraFilter>().First();

        searchParameters.id = new List<int>() { CameraFilterId };

        CameraFilterDataController.GetData(searchProperties);
        
        var cameraFilterElementData = CameraFilterDataController.Data.dataList.FirstOrDefault();

        if (cameraFilterElementData == null)
        {
            cameraFilterElementData = new CameraFilterElementData()
            {
                Name = "None",
                IconPath = "Textures/Icons/CameraFilters/None"
            };

            cameraFilterElementData.SetOriginalValues();

            CameraFilterDataController.Data.dataList = new List<IElementData>() { cameraFilterElementData };
        }

        cameraFilterElementData.DataElement = cameraFilterButton.DataElement;

        //Assign data
        var cameraFilterData = new Data()
        {
            dataController = CameraFilterDataController,
            dataList = new List<IElementData>() { cameraFilterElementData },
            searchProperties = CameraFilterDataController.SearchProperties
        };

        cameraFilterElementData.DataElement.Data = cameraFilterData;
        cameraFilterElementData.DataElement.Id = CameraFilterId;

        cameraFilterElementData.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        SetCameraFilterData();

        cameraFilterButton.DataElement.InitializeElement();
        cameraFilterButton.GetComponent<ExPanel>().InitializeChildElement();
    }
    
    private void SetCameraFilterData()
    {
        CameraFilterElementData.Id = CameraFilterId;
        cameraFilterButton.child.DataElement.Id = CameraFilterId;

        InitializeSearchParameters();
    }

    private void InitializeSearchParameters()
    {
        var searchParameters = CameraFilterDataController.SearchProperties.searchParameters.Cast<Search.CameraFilter>().First();

        searchParameters.excludeId = new List<int>() { CameraFilterId };

        searchParameters.includeEmptyElement = CameraFilterId != 0;
    }

    public void OpenSegment()
    {
        SelectionElementManager.Add(cameraFilterButton);
        SelectionManager.SelectData(cameraFilterButton.DataElement.Data.dataList);

        SetSceneActorButton();
    }

    private void SetSceneActorButton()
    {
        cameraFilterButton.DataElement.SetElement();
    }

    public void SetSearchResult(IElementData elementData)
    {
        switch (elementData.DataType)
        {
            case Enums.DataType.CameraFilter:

                var cameraFilterElementData = (CameraFilterElementData)elementData;
                UpdateCameraFilter(cameraFilterElementData);

                break;

            default: Debug.Log("CASE MISSING: " + elementData.DataType); break;
        }
    }

    public void UpdateCameraFilter(CameraFilterElementData cameraFilterElementData)
    {
        CameraFilterId = cameraFilterElementData.Id;

        SetCameraFilterData();

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment()
    {
        SetSceneActorButton();
    }

    public void CloseSegment()
    {
        SelectionElementManager.Remove(cameraFilterButton);
    }
}
