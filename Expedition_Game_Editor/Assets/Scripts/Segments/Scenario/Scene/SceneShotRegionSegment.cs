using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneShotRegionSegment : MonoBehaviour, ISegment
{
    public EditorElement regionButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneEditor SceneEditor             { get { return (SceneEditor)DataEditor; } }

    #region Data properties
    private int Id
    {
        get { return SceneEditor.Id; }
    }

    //private int DefaultRegionId
    //{
    //    get { return SceneEditor.DefaultRegionId; }
    //    set { SceneEditor.DefaultRegionId = value; }
    //}

    //private string LocationName
    //{
    //    get { return SceneEditor.LocationName; }
    //    set { SceneEditor.LocationName = value; }
    //}
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeSceneButton();
    }

    private void InitializeSceneButton()
    {
        //Assign data
        //regionButton.DataElement.Id = DataEditor.ElementData.Id;
        //regionButton.DataElement.Data = DataEditor.Data;

        //regionButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        //regionButton.InitializeElement();
    }

    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
