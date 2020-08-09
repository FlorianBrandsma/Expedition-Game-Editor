using UnityEngine;
using System.Linq;

public class InteractionDestinationArrivalRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExToggle freeRotationToggle;
    public ExInputNumber xInputField, yInputField, zInputField;

    private bool freeRotation;
    private int rotationX, rotationY, rotationZ;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public bool FreeRotation
    {
        get { return freeRotation; }
        set
        {
            freeRotation = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.FreeRotation = value;
            });
        }
    }

    public int RotationX
    {
        get { return rotationX; }
        set
        {
            rotationX = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.RotationX = value;
            });
        }
    }

    public int RotationY
    {
        get { return rotationY; }
        set
        {
            rotationY = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.RotationY = value;
            });
        }
    }

    public int RotationZ
    {
        get { return rotationZ; }
        set
        {
            rotationZ = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.RotationZ = value;
            });
        }
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var interactionDestinationData = (InteractionDestinationElementData)DataEditor.Data.elementData;

        freeRotation = interactionDestinationData.FreeRotation;

        rotationX = interactionDestinationData.RotationX;
        rotationY = interactionDestinationData.RotationY;
        rotationZ = interactionDestinationData.RotationZ;
    }

    public void InitializeSegment() { }

    public void UpdateFreeRotation()
    {
        FreeRotation = freeRotationToggle.Toggle.isOn;

        EnableInputFields(!FreeRotation);

        DataEditor.UpdateEditor();
    }

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
    
    public void OpenSegment()
    {
        freeRotationToggle.Toggle.isOn = FreeRotation;

        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        EnableInputFields(!FreeRotation);

        gameObject.SetActive(true);
    }

    private void EnableInputFields(bool enable)
    {
        xInputField.EnableElement(enable);
        yInputField.EnableElement(enable);
        zInputField.EnableElement(enable);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
