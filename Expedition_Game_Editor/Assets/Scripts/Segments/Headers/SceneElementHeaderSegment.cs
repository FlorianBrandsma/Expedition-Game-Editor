using UnityEngine;
using UnityEngine.UI;

public class SceneElementHeaderSegment : MonoBehaviour, ISegment
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
        header = interactionData.Description;
        objectGraphicIconPath = interactionData.objectGraphicIconPath;

        headerText.fontSize = StyleManager.baseFontSize;
        headerText.resizeTextMaxSize = StyleManager.baseFontSize;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        id = sceneObjectData.Id;
        header = sceneObjectData.objectGraphicName;
        objectGraphicIconPath = sceneObjectData.objectGraphicIconPath;

        headerText.fontSize = StyleManager.headerFontSize;
        headerText.resizeTextMaxSize = StyleManager.headerFontSize;
    }

    public void OpenSegment()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction: InitializeInteractionData(); break;
            case Enums.DataType.SceneObject: InitializeSceneObjectData(); break;
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