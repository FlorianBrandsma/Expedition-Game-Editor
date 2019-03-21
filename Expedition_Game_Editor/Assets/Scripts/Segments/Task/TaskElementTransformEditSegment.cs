using UnityEngine;
using System.Collections;

public class TaskElementTransformEditSegment : MonoBehaviour
{
    private SegmentController segment;

    public SelectionElement editor_button;

    private void Awake()
    {
        segment = GetComponent<SegmentController>();
    }

    public void OpenEditor()
    {
        SetEditorButton();
    }

    private void SetEditorButton()
    {
        editor_button.segmentController = segment;
    }

    public void SaveEdit()
    {
        ApplyEdit();
        CancelEdit();
    }

    public void ApplyEdit()
    {

    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {

    }
}
