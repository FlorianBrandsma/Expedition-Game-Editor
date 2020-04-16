﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class InteractionGeneralInteractableStatusSegment : MonoBehaviour, ISegment
{
    private InteractionDataElement InteractionData { get { return (InteractionDataElement)DataEditor.Data.dataElement; } }

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
        var regionData = new RegionDataElement();

        regionData.Id = InteractionData.RegionId;
        regionData.DataType = Enums.DataType.Region;
        regionData.type = Enums.RegionType.Interaction;
        
        var regionSearchParameters = new Search.Region();

        var phaseRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Phase);

        //To get all phase regions
        if (phaseRoute != null)
            regionSearchParameters.phaseId = new List<int>() { phaseRoute.GeneralData.Id };
        else
            regionSearchParameters.phaseId = new List<int>() { 0 };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, new[] { regionSearchParameters });

        if (regionData.Id == 0)
            regionData.Id = SegmentController.DataController.DataList.FirstOrDefault().Id;
        
        editButton.path = SegmentController.editorController.PathController.route.path;
        
        editButton.data = new SelectionElement.Data(SegmentController.DataController, regionData);
        buttonIcon.texture = Resources.Load<Texture2D>(InteractionData.objectGraphicIconPath);
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
