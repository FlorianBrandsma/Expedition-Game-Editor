using UnityEngine;
using UnityEngine.UI;

public class OutcomeHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text interactionTimeFrameText;
    public Text taskNameText;
    public Text idText;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public OutcomeEditor OutcomeEditor { get { return (OutcomeEditor)DataEditor; } }

    #region Data properties
    public int Id
    {
        get { return OutcomeEditor.Id; }
    }

    private string ModelIconPath
    {
        get { return OutcomeEditor.ModelIconPath; }
    }

    private bool DefaultInteraction
    {
        get { return OutcomeEditor.DefaultInteraction; }
    }

    private int InteractionStartTime
    {
        get { return OutcomeEditor.InteractionStartTime; }
    }

    private int InteractionEndTime
    {
        get { return OutcomeEditor.InteractionEndTime; }
    }

    private string TaskName
    {
        get { return OutcomeEditor.TaskName; }
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
        InitializeDependencies();

        if (DataEditor.Loaded) return;
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        idText.text = Id.ToString();

        icon.texture = Resources.Load<Texture2D>(ModelIconPath);

        interactionTimeFrameText.text = DefaultInteraction ? "Default" : TimeManager.FormatTime(InteractionStartTime) + " - " + TimeManager.FormatTime(InteractionEndTime);
        taskNameText.text = TaskName;
        
        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
