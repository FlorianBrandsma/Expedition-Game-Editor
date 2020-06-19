﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class PhasePartyDefaultStatusSegment : MonoBehaviour, ISegment
{
    private PhaseDataElement PhaseData { get { return (PhaseDataElement)DataEditor.Data.dataElement; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text nameText;
    public Text stateText;
    public Text locationText;

    public SelectionElement editButton;
    public RawImage buttonIcon;
    #endregion

    #region Data Methods
    private void InitializeEditButton()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.phaseId = new List<int>() { PhaseData.Id };
        
        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
        
        var regionData = SegmentController.DataController.DataList.Cast<RegionDataElement>().Where(x => x.Id == PhaseData.DefaultRegionId).FirstOrDefault();
        regionData.type = Enums.RegionType.Party;

        editButton.path = SegmentController.editorController.PathController.route.path;

        nameText.text = PhaseData.interactableName;
        locationText.text = PhaseData.locationName;

        editButton.data = new SelectionElement.Data(SegmentController.DataController, regionData);
        buttonIcon.texture = Resources.Load<Texture2D>(PhaseData.objectGraphicIconPath);
    }

    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        InitializeEditButton();
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}