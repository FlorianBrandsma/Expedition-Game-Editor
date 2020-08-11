using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class PhaseGeneralDefaultValuesSegment : MonoBehaviour, ISegment
{
    private PhaseElementData PhaseData { get { return (PhaseElementData)DataEditor.Data.elementData; } }
    
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
        
        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
        
        var regionData = SegmentController.DataController.DataList.Cast<RegionElementData>().Where(x => x.Id == PhaseData.DefaultRegionId).FirstOrDefault();
        regionData.type = Enums.RegionType.Party;

        editButton.Path = SegmentController.EditorController.PathController.route.path;

        editButton.data = new DataElement.Data(SegmentController.DataController, regionData);
        buttonIcon.texture = Resources.Load<Texture2D>(PhaseData.objectGraphicIconPath);
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        nameText.text = PhaseData.interactableName;
        locationText.text = PhaseData.locationName;
        timeText.text = TimeManager.FormatTime(PhaseData.DefaultTime);

        InitializeEditButton();
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}