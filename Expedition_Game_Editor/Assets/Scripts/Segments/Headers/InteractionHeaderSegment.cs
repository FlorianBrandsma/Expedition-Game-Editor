using UnityEngine;
using UnityEngine.UI;

public class InteractionHeaderSegment : MonoBehaviour, ISegment
{
    private InteractionDataElement InteractionData { get { return (InteractionDataElement)DataEditor.Data.dataElement; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public RawImage icon;
    public InputField inputField;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private string description;
    private string objectGraphicIcon;
    #endregion

    #region Data Properties
    public string Description
    {
        get { return description; }
        set
        {
            description = value;

            InteractionData.Description = value;
        }
    }
    #endregion

    #region Data Methods
    public void UpdateDescription()
    {
        Description = inputField.text;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        id = InteractionData.Id;
        index = InteractionData.Index;
        description = InteractionData.Description;
        objectGraphicIcon = InteractionData.objectGraphicIconPath;

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, InteractionData.Index);
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputField.text = description;

        icon.texture = Resources.Load<Texture2D>(objectGraphicIcon);

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
