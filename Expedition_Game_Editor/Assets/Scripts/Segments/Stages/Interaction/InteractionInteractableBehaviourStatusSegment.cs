using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class InteractionInteractableBehaviourStatusSegment : MonoBehaviour, ISegment
{
    private InteractionDataElement InteractionData { get { return (InteractionDataElement)DataEditor.Data.dataElement; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text nameText;
    public Text stateText;
    public Text locationText;

    public DataElement editButton;
    public RawImage buttonIcon;
    #endregion

    #region Data Methods
    private void InitializeEditButton()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        var phaseRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Phase);

        //Get all phase regions if a phase was selected
        if (phaseRoute != null)
            searchParameters.phaseId = new List<int>() { phaseRoute.GeneralData.Id };
        else
            searchParameters.phaseId = new List<int>() { 0 };
        
        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
        
        var regionData = SegmentController.DataController.DataList.Cast<RegionDataElement>().Where(x => x.Id == InteractionData.RegionId).FirstOrDefault();
        regionData.type = Enums.RegionType.Interaction;

        editButton.Path = SegmentController.EditorController.PathController.route.path;

        nameText.text = InteractionData.interactableName;
        locationText.text = InteractionData.locationName; 

        editButton.data = new DataElement.Data(SegmentController.DataController, regionData);
        buttonIcon.texture = Resources.Load<Texture2D>(InteractionData.objectGraphicIconPath);
    }

    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

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

    public void SetSearchResult(DataElement selectionElement) { }
    #endregion
}
