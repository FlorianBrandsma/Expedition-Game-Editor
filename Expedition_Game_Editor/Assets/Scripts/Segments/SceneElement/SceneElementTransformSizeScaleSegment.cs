using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SceneElementTransformSizeScaleSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public EditorInputNumber inputField;

    public Text heightText, widthText, depthText;
    #endregion

    #region Data Variables
    private float height;
    private float width;
    private float depth;
    private float scaleMultiplier;
    #endregion

    #region Properties
    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            scaleMultiplier = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.ScaleMultiplier = value;
                    });

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectDataList = DataEditor.DataList.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectDataList.ForEach(sceneObjectData =>
                    {
                        sceneObjectData.ScaleMultiplier = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    #region Methods
    public void UpdateScaleMultiplier()
    {
        ScaleMultiplier = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void SetSizeValues()
    {
        heightText.text = (height * scaleMultiplier).ToString();
        widthText.text = (width * scaleMultiplier).ToString();
        depthText.text = (depth * scaleMultiplier).ToString();
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
        SetSizeValues();
    }
    
    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction: InitializeInteractionData(); break;
            case Enums.DataType.SceneObject: InitializeSceneObjectData(); break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        scaleMultiplier = interactionData.ScaleMultiplier;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        scaleMultiplier = sceneObjectData.ScaleMultiplier;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        GetSizeData();

        inputField.Value = ScaleMultiplier;
    }

    private void GetSizeData()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction: GetInteractionSizeData(); break;
            case Enums.DataType.SceneObject: GetSceneObjectSizeData(); break;

            default: Debug.Log("CASE MISSING"); break;
        }

        SetSizeValues();
    }

    private void GetInteractionSizeData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        height = interactionData.height;
        width = interactionData.width;
        depth = interactionData.depth;
    }

    private void GetSceneObjectSizeData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        height = sceneObjectData.height;
        width = sceneObjectData.width;
        depth = sceneObjectData.depth;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
