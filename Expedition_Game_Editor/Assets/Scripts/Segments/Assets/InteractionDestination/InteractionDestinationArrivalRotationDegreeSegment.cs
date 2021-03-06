﻿using UnityEngine;

public class InteractionDestinationArrivalRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExToggle changeRotationToggle;
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractionDestinationEditor InteractionDestinationEditor { get { return (InteractionDestinationEditor)DataEditor; } }

    #region Data properties
    private bool ChangeRotation
    {
        get { return InteractionDestinationEditor.ChangeRotation; }
        set { InteractionDestinationEditor.ChangeRotation = value; }
    }

    private int RotationX
    {
        get { return InteractionDestinationEditor.RotationX; }
        set { InteractionDestinationEditor.RotationX = value; }
    }

    private int RotationY
    {
        get { return InteractionDestinationEditor.RotationY; }
        set { InteractionDestinationEditor.RotationY = value; }
    }

    private int RotationZ
    {
        get { return InteractionDestinationEditor.RotationZ; }
        set { InteractionDestinationEditor.RotationZ = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        changeRotationToggle.Toggle.isOn = ChangeRotation;

        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        UpdateSegment();

        gameObject.SetActive(true);
    }

    public void UpdateFreeRotation()
    {
        ChangeRotation = changeRotationToggle.Toggle.isOn;

        UpdateSegment();

        DataEditor.UpdateEditor();
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

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

    public void UpdateSegment()
    {
        EnableInputFields(ChangeRotation);
    }

    private void EnableInputFields(bool enable)
    {
        xInputField.EnableElement(enable);
        yInputField.EnableElement(enable);
        zInputField.EnableElement(enable);
    }

    public void CloseSegment() { }
}
