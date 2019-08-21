using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class InteractionBehaviourTransformEditSegment : MonoBehaviour, ISegment
{
    private InteractionDataElement interactionData;

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    private IDataController DataController { get { return GetComponent<IDataController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI

    public SelectionElement editButton;

    #endregion

    #region Data Methods

    private void InitializeEditButton()
    {
        interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        var regionData = new RegionDataElement();

        regionData.id = interactionData.RegionId;
        regionData.dataType = Enums.DataType.Region;
        regionData.type = Enums.RegionType.Interaction;

        editButton.path = SegmentController.editorController.PathController.route.path;

        editButton.InitializeElement(null);

        var searchParameters = new Search.Region();

        var phaseRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Phase);

        //To get all phase regions
        if (phaseRoute != null)
            searchParameters.phaseId = new List<int>() { phaseRoute.GeneralData().id };
        else
            searchParameters.phaseId = new List<int>() { 0 };

        DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });

        if(regionData.id == 0)
            regionData.id = DataController.DataList.FirstOrDefault().Id;

        editButton.data = new SelectionElement.Data(DataController, regionData);

        editButton.GetComponentInChildren<Text>().text = interactionData.regionName != "" ? "Open " + interactionData.regionName : "Set Region";
    }

    #endregion

    #region Segment

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
        InitializeEditButton();
    }

    public void InitializeData()
    {
        
    }

    public void OpenSegment()
    {
        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {
        
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }

    #endregion
}
