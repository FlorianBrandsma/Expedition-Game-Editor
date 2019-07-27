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
        interactionData = (InteractionDataElement)DataEditor.Data.DataElement;

        var regionData = new RegionDataElement();

        regionData.id = interactionData.RegionId;
        regionData.dataType = Enums.DataType.Region;
        regionData.type = Enums.RegionType.Interaction;

        editButton.route.path = SegmentController.editorController.PathController.route.path;

        editButton.InitializeElement(null);

        var searchParameters = new Search.Region();

        //To get all phase regions
        if (SegmentController.Path.FindLastRoute(Enums.DataType.Phase) != null)
            searchParameters.phaseId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Phase).GeneralData().id };

        DataController.GetData(new[] { searchParameters });

        editButton.route.data = new Data(DataController, regionData);

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
