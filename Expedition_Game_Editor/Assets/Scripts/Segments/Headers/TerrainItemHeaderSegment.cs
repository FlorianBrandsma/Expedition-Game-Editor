using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainItemHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    //private InteractionDataElement interactionData;

    public IEditor DataEditor { get; set; }

    #region UI

    public Button findButton;
    public Text idText;

    #endregion

    #region Data Variables

    private int id;
    private string objectGraphicIconPath;

    #endregion

    #region Data Methods
    public void UpdateDescription()
    {
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();
    }

    public void InitializeData()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.SceneObject:    InitializeSceneObjectData();    break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        id = interactionData.id;
        objectGraphicIconPath = interactionData.objectGraphicIconPath;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        id = sceneObjectData.id;
        objectGraphicIconPath = sceneObjectData.objectGraphicIconPath;
    }

    public void OpenSegment()
    {
        idText.text = id.ToString();

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
    #endregion
}