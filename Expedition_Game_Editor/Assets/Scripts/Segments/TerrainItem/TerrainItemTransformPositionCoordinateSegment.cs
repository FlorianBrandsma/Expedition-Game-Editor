using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TerrainItemTransformPositionCoordinateSegment : MonoBehaviour, ISegment
{
    private SceneDataElement sceneDataElement;

    private SegmentController SegmentController     { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    private InteractableController ElementController{ get { return (InteractableController)SegmentController.DataController; } }

    #region UI

    public EditorInputNumber xInputField, yInputField, zInputField;
    public EditorToggle bindToTile;

    #endregion

    #region Data Variables

    private float positionX, positionY, positionZ;
    private int terrainTileId;

    #endregion

    #region Data Properties

    public float PositionX
    {
        get { return positionX; }
        set
        {
            positionX = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;
                    interactionData.PositionX = value;

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;
                    sceneObjectData.PositionX = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            positionY = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;
                    interactionData.PositionY = value;

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;
                    sceneObjectData.PositionY = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            positionZ = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;
                    interactionData.PositionZ = value;

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;
                    sceneObjectData.PositionZ = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    public void UpdatePositionX()
    {
        PositionX = xInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionY()
    {
        PositionY = yInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionZ()
    {
        PositionZ = zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateTile()
    {
        //Debug.Log(bindToTile.Toggle.isOn);
    }

    static public int GetTile()
    {
        return 0;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeData()
    {
        var regionData = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;
        sceneDataElement = regionData.sceneDataElement;

        //Debug.Log(sceneDataElement.tileSize);

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.SceneObject:    InitializeSceneObjectData();    break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        positionX = interactionData.PositionX;
        positionY = interactionData.PositionY;
        positionZ = interactionData.PositionZ;

        terrainTileId = interactionData.TerrainTileId;
    }

    private void InitializeSceneObjectData()
    {
        var sceneObjectData = (SceneObjectDataElement)DataEditor.Data.dataElement;

        positionX = sceneObjectData.PositionX;
        positionY = sceneObjectData.PositionY;
        positionZ = sceneObjectData.PositionZ;

        terrainTileId = sceneObjectData.TerrainTileId;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;

        bindToTile.Toggle.isOn = terrainTileId != 0;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
