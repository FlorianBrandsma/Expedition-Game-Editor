using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SceneElementTransformRotationDegreeSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController     { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    private InteractableController ElementController{ get { return (InteractableController)SegmentController.DataController; } }

    #region UI

    public EditorInputNumber xInputField, yInputField, zInputField;

    #endregion

    #region Data Variables

    private int rotationX, rotationY, rotationZ;

    #endregion

    #region Data Properties

    public int RotationX
    {
        get { return rotationX; }
        set
        {
            rotationX = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = DataEditor.DataElements.Cast<InteractionDataElement>().ToList();
                    interactionData.ForEach(x => x.RotationX = value);

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = DataEditor.DataElements.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectData.ForEach(x => x.RotationX = value);

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public int RotationY
    {
        get { return rotationY; }
        set
        {
            rotationY = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = DataEditor.DataElements.Cast<InteractionDataElement>().ToList();
                    interactionData.ForEach(x => x.RotationY = value);

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = DataEditor.DataElements.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectData.ForEach(x => x.RotationY = value);

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public int RotationZ
    {
        get { return rotationZ; }
        set
        {
            rotationZ = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = DataEditor.DataElements.Cast<InteractionDataElement>().ToList();
                    interactionData.ForEach(x => x.RotationZ = value);

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = DataEditor.DataElements.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectData.ForEach(x => x.RotationZ = value);

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    public void UpdateRotationX()
    {
        RotationX = (int)xInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationY()
    {
        RotationY = (int)yInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationZ()
    {
        RotationZ = (int)zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void ApplySegment() { }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();
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

        rotationX = interactionData.RotationX;
        rotationY = interactionData.RotationY;
        rotationZ = interactionData.RotationZ;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        rotationX = sceneObjectData.RotationX;
        rotationY = sceneObjectData.RotationY;
        rotationZ = sceneObjectData.RotationZ;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        DataEditor.DataElements.ForEach(x => x.ClearChanges());
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
