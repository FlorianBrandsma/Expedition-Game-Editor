using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldElementTransformRotationDegreeSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber xInputField, yInputField, zInputField;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.RotationX = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.RotationX = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseDataElement>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultRotationX = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.RotationY = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.RotationY = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseDataElement>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultRotationY = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.RotationZ = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.RotationZ = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseDataElement>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultRotationZ = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }
    #endregion

    #region Methods
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
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

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
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.WorldObject:    InitializeWorldObjectData();    break;
            case Enums.DataType.Phase:          InitializePhaseData();          break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        rotationX = interactionData.RotationX;
        rotationY = interactionData.RotationY;
        rotationZ = interactionData.RotationZ;
    }

    private void InitializeWorldObjectData()
    {
        var worldObjectData = (WorldObjectDataElement)DataEditor.Data.dataElement;

        rotationX = worldObjectData.RotationX;
        rotationY = worldObjectData.RotationY;
        rotationZ = worldObjectData.RotationZ;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        rotationX = phaseData.DefaultRotationX;
        rotationY = phaseData.DefaultRotationY;
        rotationZ = phaseData.DefaultRotationZ;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
    #endregion
}
