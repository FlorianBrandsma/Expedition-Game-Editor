using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneActorActSpeechMethodSegment : MonoBehaviour, ISegment
{
    public ExDropdown speechMethodDropdown;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneActorEditor SceneActorEditor   { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    public int SpeechMethod
    {
        get { return SceneActorEditor.SpeechMethod; }
        set { SceneActorEditor.SpeechMethod = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        speechMethodDropdown.Dropdown.captionText.text = Enum.GetName(typeof(Enums.SpeechMethod), SpeechMethod);

        foreach (var display in Enum.GetValues(typeof(Enums.SpeechMethod)))
        {
            speechMethodDropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }
    }

    public void OpenSegment()
    {
        speechMethodDropdown.Dropdown.value = SpeechMethod;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSpeechMethod()
    {
        SpeechMethod = speechMethodDropdown.Dropdown.value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        speechMethodDropdown.Dropdown.options.Clear();
    }
}