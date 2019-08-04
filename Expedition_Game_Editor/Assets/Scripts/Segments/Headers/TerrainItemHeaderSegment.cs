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
            case Enums.DataType.TerrainObject:  InitializeTerrainObjectData();  break;
        }
    }

    private void InitializeInteractionData()
    {
        var terrainInteractableData = (InteractionDataElement)DataEditor.Data.dataElement;

        id = terrainInteractableData.id;
        objectGraphicIconPath = terrainInteractableData.objectGraphicIconPath;
    }

    private void InitializeTerrainObjectData()
    {
        var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.dataElement;

        id = terrainObjectData.id;
        objectGraphicIconPath = terrainObjectData.objectGraphicIconPath;
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