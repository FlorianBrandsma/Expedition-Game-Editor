using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;

public class TerrainGeneralEditSegment : MonoBehaviour, ISegment
{
    #region UI

    public SelectionElement editButton;

    #endregion

    #region Data Variables

    #endregion

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    #region Methods

    private void InitializeData()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    private void InitializeEditButton()
    {
        var data = SegmentController.path.FindLastRoute("Region").data;
        var regionData = (RegionDataElement)data.DataElement;

        editButton.route.path = SegmentController.editorController.pathController.route.path;

        editButton.InitializeElement();

        editButton.route.data = new Data(data.DataController, regionData);

        editButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(SelectionManager.Property), editButton.selectionProperty) + " " + regionData.Name;
    }

    #endregion

    #region Segment

    public void InitializeSegment()
    {
        InitializeData();
        InitializeEditButton();
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
