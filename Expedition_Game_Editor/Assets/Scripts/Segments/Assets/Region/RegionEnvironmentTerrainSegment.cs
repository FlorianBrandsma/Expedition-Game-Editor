using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RegionEnvironmentTerrainSegment : MonoBehaviour, ISegment
{
    public Dropdown tileSetDropdown;

    private List<TileSetBaseData> tileSetList;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public RegionEditor RegionEditor            { get { return (RegionEditor)DataEditor; } }
    
    public int TileSetId
    {
        get { return RegionEditor.TileSetId; }
        set { RegionEditor.TileSetId = value; }
    }

    public string TileIconPath
    {
        get { return RegionEditor.TileIconPath; }
        set { RegionEditor.TileIconPath = value; }
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        tileSetDropdown.ClearOptions();
        tileSetDropdown.onValueChanged.RemoveAllListeners();

        tileSetList = DataManager.GetTileSetData();

        foreach (TileSetBaseData tileSetData in tileSetList)
            tileSetDropdown.options.Add(new Dropdown.OptionData(tileSetData.Name));

        int selectedIndex = tileSetList.FindIndex(x => x.Id == TileSetId);

        tileSetDropdown.value = selectedIndex;
        tileSetDropdown.captionText.text = tileSetDropdown.options[selectedIndex].text;

        tileSetDropdown.onValueChanged.AddListener(delegate { SetDropdown(tileSetList[tileSetDropdown.value].Id); });
    }

    public void OpenSegment()
    {
        SetDropdown(TileSetId);
    }
    
    public void SetDropdown(int tileSetId)
    {
        TileSetId = tileSetList[tileSetDropdown.value].Id;

        DataEditor.UpdateEditor();

        SetDisplay();
    }
    
    private void SetDisplay()
    {
        SetTiles();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    private void SetTiles()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Tile);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Tile>().First();
        searchParameters.tileSetId = new List<int>() { TileSetId };

        SegmentController.DataController.GetData(searchProperties);

        TileIconPath = SegmentController.DataController.Data.dataList.Cast<TileElementData>().First().Icon;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}