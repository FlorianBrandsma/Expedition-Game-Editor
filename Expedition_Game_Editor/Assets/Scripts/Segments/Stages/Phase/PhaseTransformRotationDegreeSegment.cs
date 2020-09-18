using UnityEngine;

public class PhaseTransformRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public PhaseEditor PhaseEditor              { get { return (PhaseEditor)DataEditor; } }

    #region Data properties
    private int DefaultRotationX
    {
        get { return PhaseEditor.DefaultRotationX; }
        set { PhaseEditor.DefaultRotationX = value; }
    }

    private int DefaultRotationY
    {
        get { return PhaseEditor.DefaultRotationY; }
        set { PhaseEditor.DefaultRotationY = value; }
    }

    private int DefaultRotationZ
    {
        get { return PhaseEditor.DefaultRotationZ; }
        set { PhaseEditor.DefaultRotationZ = value; }
    }

    private int DefaultTime
    {
        get { return PhaseEditor.DefaultTime; }
        set { PhaseEditor.DefaultTime = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        TimeManager.instance.ActiveTime = DefaultTime;
    }

    public void InitializeSegment()
    {
        UpdateTime();
    }

    public void OpenSegment()
    {
        xInputField.Value = DefaultRotationX;
        yInputField.Value = DefaultRotationY;
        zInputField.Value = DefaultRotationZ;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateRotationX()
    {
        DefaultRotationX = (int)xInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationY()
    {
        DefaultRotationY = (int)yInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationZ()
    {
        DefaultRotationZ = (int)zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateTime()
    {
        DefaultTime = TimeManager.instance.ActiveTime;

        DataEditor.UpdateEditor();
    }
    
    public void CloseSegment() { }
}
