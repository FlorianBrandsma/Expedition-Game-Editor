using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SceneElementTransformScaleMultiplierSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private InteractableController ElementController { get { return (InteractableController)SegmentController.DataController; } }

    #region UI

    public EditorInputNumber inputField;

    #endregion

    #region Data Variables

    private float scaleMultiplier;

    #endregion

    #region Data Properties

    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            scaleMultiplier = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = DataEditor.DataElements.Cast<InteractionDataElement>().ToList();
                    interactionData.ForEach(x => x.ScaleMultiplier = value);

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = DataEditor.DataElements.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectData.ForEach(x => x.ScaleMultiplier = value);

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    #endregion

    public void UpdateScaleMultiplier()
    {
        ScaleMultiplier = inputField.Value;

        DataEditor.UpdateEditor();
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeData()
    {
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

        gameObject.SetActive(true);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
