using UnityEngine;
using System.Collections;

public class TaskElementTransformEditSegment : MonoBehaviour, IEditor
{
    private EditorSegment segment;

    public SelectionElement editor_button;

    private void Awake()
    {
        segment = GetComponent<EditorSegment>();
    }

    public void OpenEditor()
    {
        SetEditorButton();
    }

    private void SetEditorButton()
    {
        editor_button.controller = segment.subController.controller;
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
