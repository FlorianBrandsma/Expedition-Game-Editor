using UnityEngine;
using System.Linq;

public class PhaseTransformRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    private int rotationX, rotationY, rotationZ;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public int RotationX
    {
        get { return rotationX; }
        set
        {
            rotationX = value;

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultRotationX = value;
            });
        }
    }

    public int RotationY
    {
        get { return rotationY; }
        set
        {
            rotationY = value;

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultRotationY = value;
            });
        }
    }

    public int RotationZ
    {
        get { return rotationZ; }
        set
        {
            rotationZ = value;

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultRotationZ = value;
            });
        }
    }

    public int Time
    {
        set
        {
            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultTime = value;
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

        var phaseData = (PhaseElementData)DataEditor.Data.elementData;

        rotationX = phaseData.DefaultRotationX;
        rotationY = phaseData.DefaultRotationY;
        rotationZ = phaseData.DefaultRotationZ;

        TimeManager.instance.ActiveTime = phaseData.DefaultTime;
    }

    public void InitializeSegment()
    {
        UpdateTime();
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

    public void UpdateTime()
    {
        Time = TimeManager.instance.ActiveTime;

        DataEditor.UpdateEditor();
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

    public void SetSearchResult(DataElement dataElement) { }
}
