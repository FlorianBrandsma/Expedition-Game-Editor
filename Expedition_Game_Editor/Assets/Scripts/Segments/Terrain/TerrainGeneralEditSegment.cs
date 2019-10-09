using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;

public class TerrainGeneralEditSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public SelectionElement editButton;
    #endregion

    #region Data Variables
    #endregion

    #region Methods
    private void InitializeEditButton()
    {
        var data = SegmentController.Path.FindLastRoute(Enums.DataType.Region).data;
        var regionData = (RegionDataElement)data.dataElement;

        editButton.path = SegmentController.editorController.PathController.route.path;

        editButton.InitializeElement(null);

        editButton.data = new SelectionElement.Data(data.dataController, regionData);

        editButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(SelectionManager.Property), editButton.selectionProperty) + " " + regionData.Name;
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
        InitializeEditButton();
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
