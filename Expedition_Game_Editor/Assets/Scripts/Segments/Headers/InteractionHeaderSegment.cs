using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class InteractionHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    private InteractionDataElement interactionData;

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

            interactionData.Description = value;
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
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.DataController.DataList.Count - 1);
    }

    public void InitializeData()
    {
        interactionData = (InteractionDataElement)DataEditor.Data.DataElement;

        id = interactionData.id;
        index = interactionData.Index;
        description = interactionData.Description;
        objectGraphicIcon = interactionData.objectGraphicIconPath;
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

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
    #endregion
}
