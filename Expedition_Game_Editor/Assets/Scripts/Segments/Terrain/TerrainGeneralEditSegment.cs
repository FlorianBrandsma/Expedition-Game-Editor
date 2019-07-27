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

    private void InitializeEditButton()
    {
        var data = SegmentController.Path.FindLastRoute(Enums.DataType.Region).data;
        var regionData = (RegionDataElement)data.DataElement;

        editButton.route.path = SegmentController.editorController.PathController.route.path;

        editButton.InitializeElement(null);

        editButton.route.data = new Data(data.DataController, regionData);

        editButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(SelectionManager.Property), editButton.selectionProperty) + " " + regionData.Name;
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
