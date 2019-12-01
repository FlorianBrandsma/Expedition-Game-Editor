using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SceneElementTransformScaleMultiplierSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public EditorInputNumber inputField;
    #endregion

    #region Data Variables
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

        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }
    
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
        inputField.Value = ScaleMultiplier;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
