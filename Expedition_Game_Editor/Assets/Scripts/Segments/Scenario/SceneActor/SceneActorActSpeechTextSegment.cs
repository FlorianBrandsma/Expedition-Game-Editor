using UnityEngine;

public class SceneActorActSpeechTextSegment : MonoBehaviour, ISegment
{
    public ExToggle showTextBoxToggle;
    public ExInputText speechTextInputText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneActorEditor SceneActorEditor   { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    private bool ShowTextBox
    {
        get { return SceneActorEditor.ShowTextBox; }
        set { SceneActorEditor.ShowTextBox = value; }
    }

    private string SpeechText
    {
        get { return SceneActorEditor.SpeechText; }
        set { SceneActorEditor.SpeechText = value; }
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
        showTextBoxToggle.Toggle.isOn = ShowTextBox;
        speechTextInputText.InputField.text = SpeechText;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateShowTextBox()
    {
        ShowTextBox = showTextBoxToggle.Toggle.isOn;
        
        DataEditor.UpdateEditor();
    }

    public void UpdateSpeechText()
    {
        SpeechText = speechTextInputText.InputField.text;
        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
