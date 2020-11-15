using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneShotRegionSegment : MonoBehaviour, ISegment
{
    public EditorElement regionButton;

    private RegionDataController RegionDataController   { get { return GetComponent<RegionDataController>(); } }
    private RegionElementData RegionElementData         { get { return (RegionElementData)regionButton.DataElement.ElementData; } }
    
    public SegmentController SegmentController          { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                           { get; set; }

    private SceneEditor SceneEditor                     { get { return (SceneEditor)DataEditor; } }
    
    #region Data properties
    private int Id
    {
        get { return SceneEditor.Id; }
    }

    private int RegionId
    {
        get { return SceneEditor.RegionId; }
        set { SceneEditor.RegionId = value; }
    }

    private int PhaseId
    {
        get { return SceneEditor.PhaseId; }
    }

    private string TileIconPath
    {
        get { return SceneEditor.TileIconPath; }
        set { SceneEditor.TileIconPath = value; }
    }

    private string RegionName
    {
        get { return SceneEditor.RegionName; }
        set { SceneEditor.RegionName = value; }
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
        //Get region data
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.id = new List<int>() { RegionId };
        searchParameters.type = Enums.RegionType.Scene;

        RegionDataController.GetData(searchProperties);

        var regionElementData = RegionDataController.Data.dataList.FirstOrDefault();
        regionElementData.DataElement = regionButton.DataElement;

        //Assign data
        var regionData = new Data()
        {
            dataController = RegionDataController,
            dataList = new List<IElementData>() { regionElementData },
            searchProperties = RegionDataController.SearchProperties
        };

        regionElementData.DataElement.Data = regionData;
        regionElementData.DataElement.Id = RegionId;

        regionElementData.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        SetRegionData();

        regionButton.DataElement.InitializeElement();
        regionButton.GetComponent<ExPanel>().InitializeChildElement();
    }
    
    private void SetRegionData()
    {
        RegionElementData.Id = RegionId;
        RegionElementData.TileIconPath = TileIconPath;
        RegionElementData.Name = RegionName;
        
        regionButton.child.DataElement.Id = RegionId;

        InitializeSearchParameters();
    }

    private void InitializeSearchParameters()
    {
        var searchParameters = RegionDataController.SearchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.excludeId = new List<int>() { RegionId };
        searchParameters.phaseId = new List<int>() { PhaseId };
    }

    public void OpenSegment()
    {
        SelectionElementManager.Add(regionButton);
        SelectionElementManager.Add(regionButton.child);
        SelectionManager.SelectData(regionButton.DataElement.Data.dataList);

        regionButton.DataElement.SetElement();
        regionButton.SetOverlay();
    }

    public void SetSearchResult(IElementData elementData)
    {
        switch (elementData.DataType)
        {
            case Enums.DataType.Region:

                var regionElementData = (RegionElementData)elementData;
                UpdateRegion(regionElementData);

                break;

            default: Debug.Log("CASE MISSING: " + elementData.DataType); break;
        }
    }

    public void UpdateRegion(RegionElementData regionElementData)
    {
        RegionId = regionElementData.Id;

        TileIconPath = regionElementData.TileIconPath;

        RegionName = regionElementData.Name;
        
        SetRegionData();

        DataEditor.UpdateEditor();
    }

    public void CloseSegment()
    {
        SelectionElementManager.elementPool.Remove(regionButton);
        SelectionElementManager.elementPool.Remove(regionButton.child);
    }
}
