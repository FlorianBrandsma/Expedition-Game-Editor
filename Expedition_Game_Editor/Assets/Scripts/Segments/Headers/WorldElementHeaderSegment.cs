using UnityEngine;
using UnityEngine.UI;

public class WorldElementHeaderSegment : MonoBehaviour, ISegment
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

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        id = interactionData.Id;
        header = interactionData.PublicNotes;
        objectGraphicIconPath = interactionData.objectGraphicIconPath;

        headerText.fontSize = StyleManager.baseFontSize;
        headerText.resizeTextMaxSize = StyleManager.baseFontSize;
    }

    private void InitializeWorldObjectData()
    {
        var worldObjectData = (WorldObjectDataElement)DataEditor.Data.dataElement;

        id = worldObjectData.Id;
        header = worldObjectData.objectGraphicName;
        objectGraphicIconPath = worldObjectData.objectGraphicIconPath;

        headerText.fontSize = StyleManager.headerFontSize;
        headerText.resizeTextMaxSize = StyleManager.headerFontSize;
    }

    public void OpenSegment()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction: InitializeInteractionData(); break;
            case Enums.DataType.WorldObject: InitializeWorldObjectData(); break;
        }
        
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