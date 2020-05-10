using UnityEngine;
using UnityEngine.UI;

public class InteractableSaveHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public RawImage icon;
    public Text headerText;
    public Text idText;
    #endregion

    #region Data Variables
    private string objectGraphicIconPath;
    private string header;
    private int id;
    #endregion

    #region Properties
    #endregion

    #region Methods
    public void UpdateDescription()
    {
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
        InitializeDependencies();

        if (DataEditor.Loaded) return;
    }

    private void InitializeInteractableData()
    {
        var interactableData = (InteractableDataElement)DataEditor.Data.dataElement;

        id = interactableData.Id;
        header = interactableData.Name;
        objectGraphicIconPath = interactableData.objectGraphicIconPath;
    }

    public void OpenSegment()
    {
        InitializeInteractableData();

        icon.texture = Resources.Load<Texture2D>(objectGraphicIconPath);
        headerText.text = header;
        idText.text = id.ToString();

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
