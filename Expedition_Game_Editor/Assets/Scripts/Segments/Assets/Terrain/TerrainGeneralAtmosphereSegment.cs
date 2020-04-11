using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;

public class TerrainGeneralAtmosphereSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public SelectionElement editButton;
    #endregion

    #region Data Variables
    #endregion

    #region Methods
    private void InitializeEditButton()
    {
        //Cut the path to the region
        editButton.path = SegmentController.Path.TrimToLastType(Enums.DataType.Region);

        //Take the data from the last selected terrain
        editButton.InitializeElement(SegmentController.Path.FindFirstRoute(Enums.DataType.Terrain).data);
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
