﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldElementTransformSizeScaleSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber inputField;

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

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.ScaleMultiplier = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseDataElement>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultScaleMultiplier = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

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
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.WorldObject:    InitializeWorldObjectData();    break;
            case Enums.DataType.Phase:          InitializePhaseData();          break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        scaleMultiplier = interactionData.ScaleMultiplier;
    }

    private void InitializeWorldObjectData()
    {
        var worldObjectData = (WorldObjectDataElement)DataEditor.Data.dataElement;

        scaleMultiplier = worldObjectData.ScaleMultiplier;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        scaleMultiplier = phaseData.DefaultScaleMultiplier;
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
            case Enums.DataType.Interaction:    GetInteractionSizeData();   break;
            case Enums.DataType.WorldObject:    GetWorldObjectSizeData();   break;
            case Enums.DataType.Phase:          GetPhaseSizeData();         break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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

    private void GetWorldObjectSizeData()
    {
        var worldObjectData = (WorldObjectDataElement)DataEditor.Data.dataElement;

        height = worldObjectData.height;
        width = worldObjectData.width;
        depth = worldObjectData.depth;
    }

    private void GetPhaseSizeData()
    {
        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        height = phaseData.height;
        width = phaseData.width;
        depth = phaseData.depth;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
    #endregion
}
