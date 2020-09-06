using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class PhaseGeneralDefaultValuesSegment : MonoBehaviour, ISegment
{
    private PhaseElementData PhaseData { get { return (PhaseElementData)DataEditor.ElementData; } }
    
    public Text nameText;
    public Text stateText;
    public Text locationText;
    public Text timeText;

    public DataElement editButton;
    public RawImage buttonIcon;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private void InitializeEditButton()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.phaseId = new List<int>() { PhaseData.Id };

        SegmentController.DataController.GetData(searchProperties);

        var regionData = SegmentController.DataController.Data.dataList.Cast<RegionElementData>().Where(x => x.Id == PhaseData.DefaultRegionId).FirstOrDefault();
        regionData.Type = Enums.RegionType.Party;

        editButton.Data = SegmentController.DataController.Data;
        editButton.Id = regionData.Id;

        editButton.Path = SegmentController.EditorController.PathController.route.path;
        
        buttonIcon.texture = Resources.Load<Texture2D>(PhaseData.ModelIconPath);
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        nameText.text = PhaseData.InteractableName;
        locationText.text = PhaseData.LocationName;
        timeText.text = TimeManager.FormatTime(PhaseData.DefaultTime);

        InitializeEditButton();
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}