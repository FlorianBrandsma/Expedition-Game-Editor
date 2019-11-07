using UnityEngine;
using UnityEngine.UI;

public class InteractionHeaderSegment : MonoBehaviour, ISegment
{
    private InteractionDataElement InteractionData { get { return (InteractionDataElement)DataEditor.Data.dataElement; } }

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public SelectionElement selectionElement;
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

        DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        id = InteractionData.Id;
        index = InteractionData.Index;
        description = InteractionData.Description;
        objectGraphicIcon = InteractionData.objectGraphicIconPath;

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index);
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputField.text = description;

        selectionElement.GetComponent<EditorTile>().icon.texture = Resources.Load<Texture2D>(objectGraphicIcon);

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
