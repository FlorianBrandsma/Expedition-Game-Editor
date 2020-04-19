using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RegionEnvironmentTerrainSegment : MonoBehaviour, ISegment
{
    private RegionDataElement RegionDataElement { get { return (RegionDataElement)DataEditor.Data.dataElement; } }

    private DataManager dataManager = new DataManager();

    private List<DataManager.TileSetData> tileSetList;

    #region UI
    public Dropdown tileSetDropdown;
    #endregion

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        InitializeDropdown();
    }

    public void InitializeData() { }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SetDropdown(RegionDataElement.TileSetId);
    }

    private void InitializeDropdown()
    {
        tileSetDropdown.ClearOptions();
        tileSetDropdown.onValueChanged.RemoveAllListeners();

        tileSetList = dataManager.GetTileSetData();

        foreach (DataManager.TileSetData tileSetData in tileSetList)
            tileSetDropdown.options.Add(new Dropdown.OptionData(tileSetData.name));

        int selectedIndex = tileSetList.FindIndex(x => x.Id == RegionDataElement.TileSetId);

        tileSetDropdown.value = selectedIndex;
        tileSetDropdown.captionText.text = tileSetDropdown.options[selectedIndex].text;

        tileSetDropdown.onValueChanged.AddListener(delegate { SetDropdown(tileSetList[tileSetDropdown.value].Id); });
    }

    public void SetDropdown(int tileSetId)
    {
        RegionDataElement.TileSetId = tileSetList[tileSetDropdown.value].Id;

        DataEditor.UpdateEditor();

        SetDisplay();
    }

    private void SetTiles()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Tile);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Tile>().First();
        searchParameters.tileSetId = new List<int>() { RegionDataElement.TileSetId };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, searchProperties);

        RegionDataElement.tileIconPath = SegmentController.DataController.DataList.Cast<TileDataElement>().First().icon;
    }

    private void SetDisplay()
    {
        SetTiles();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}