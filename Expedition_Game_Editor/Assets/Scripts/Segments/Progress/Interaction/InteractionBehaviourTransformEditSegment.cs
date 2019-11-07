using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class InteractionBehaviourTransformEditSegment : MonoBehaviour, ISegment
{
    private InteractionDataElement InteractionData { get { return (InteractionDataElement)DataEditor.Data.dataElement; } }

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public SelectionElement editButton;
    #endregion

    #region Data Methods
    private void InitializeEditButton()
    {
        var regionData = new RegionDataElement();

        regionData.Id = InteractionData.RegionId;
        regionData.DataType = Enums.DataType.Region;
        regionData.type = Enums.RegionType.Interaction;

        editButton.path = SegmentController.editorController.PathController.route.path;

        editButton.InitializeElement(null);

        var searchParameters = new Search.Region();

        var phaseRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Phase);

        //To get all phase regions
        if (phaseRoute != null)
            searchParameters.phaseId = new List<int>() { phaseRoute.GeneralData.Id };
        else
            searchParameters.phaseId = new List<int>() { 0 };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });

        if(regionData.Id == 0)
            regionData.Id = SegmentController.DataController.DataList.FirstOrDefault().Id;

        editButton.data = new SelectionElement.Data(SegmentController.DataController, regionData);

        editButton.GetComponentInChildren<Text>().text = InteractionData.regionName != "" ? "Open " + InteractionData.regionName : "Set Region";
    }

    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        InitializeData();
        InitializeEditButton();
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
