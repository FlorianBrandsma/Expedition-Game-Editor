using UnityEngine;
using UnityEngine.UI;

public class SceneItemHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Button findButton;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private string objectGraphicIconPath;
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

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.SceneObject:    InitializeSceneObjectData();    break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        id = interactionData.Id;
        objectGraphicIconPath = interactionData.objectGraphicIconPath;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        id = sceneObjectData.Id;
        objectGraphicIconPath = sceneObjectData.objectGraphicIconPath;
    }

    public void OpenSegment()
    {
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