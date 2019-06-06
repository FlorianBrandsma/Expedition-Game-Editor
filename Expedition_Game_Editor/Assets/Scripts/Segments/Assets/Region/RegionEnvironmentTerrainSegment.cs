using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RegionEnvironmentTerrainSegment : MonoBehaviour, ISegment
{
    private RegionDataElement regionDataElement;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TileSetData> tileSetList;

    public Dropdown tileSetDropdown;

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
        InitializeDropdown();
    }

    public void InitializeData()
    {
        regionDataElement = (RegionDataElement)DataEditor.Data.DataElement;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SetDropdown(regionDataElement.TileSetId);
    }

    private void InitializeDropdown()
    {
        tileSetDropdown.ClearOptions();
        tileSetDropdown.onValueChanged.RemoveAllListeners();

        tileSetList = dataManager.GetTileSetData();

        foreach (DataManager.TileSetData tileSetData in tileSetList)
            tileSetDropdown.options.Add(new Dropdown.OptionData(tileSetData.name));

        int selectedIndex = tileSetList.FindIndex(x => x.id == regionDataElement.TileSetId);

        tileSetDropdown.value = selectedIndex;
        tileSetDropdown.captionText.text = tileSetDropdown.options[selectedIndex].text;

        tileSetDropdown.onValueChanged.AddListener(delegate { SetDropdown(tileSetList[tileSetDropdown.value].id); });
    }

    public void SetDropdown(int tileSetId)
    {
        regionDataElement.TileSetId = tileSetList[tileSetDropdown.value].id;

        DataEditor.UpdateEditor();

        SetDisplay();
    }

    private void SetTiles()
    {
        var searchParameters = new Search.Tile();

        searchParameters.requestType = Search.Tile.RequestType.Custom;
        searchParameters.tileSetId = new List<int>() { regionDataElement.TileSetId };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    private void SetDisplay()
    {
        SetTiles();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}