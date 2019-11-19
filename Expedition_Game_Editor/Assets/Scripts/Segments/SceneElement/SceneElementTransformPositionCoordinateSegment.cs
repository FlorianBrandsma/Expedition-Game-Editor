using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SceneElementTransformPositionCoordinateSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public EditorInputNumber xInputField, yInputField, zInputField;
    public EditorToggle bindToTile;
    #endregion

    #region Data Variables
    private float positionX, positionY, positionZ;
    private int terrainTileId;
    #endregion

    #region Properties
    public float PositionX
    {
        get { return positionX; }
        set
        {
            positionX = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.PositionX = value;
                    });

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectDataList = DataEditor.DataList.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectDataList.ForEach(sceneObjectData =>
                    {
                        sceneObjectData.PositionX = value;
                    });

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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.PositionY = value;
                    });

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectDataList = DataEditor.DataList.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectDataList.ForEach(sceneObjectData =>
                    {
                        sceneObjectData.PositionY = value;
                    });

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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    { 
                        interactionData.PositionZ = value;
                    });

                    break;

                case Enums.DataType.SceneObject:

                    var sceneObjectDataList = DataEditor.DataList.Cast<SceneObjectDataElement>().ToList();
                    sceneObjectDataList.ForEach(sceneObjectData =>
                    {
                        sceneObjectData.PositionZ = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    #region Methods
    public void UpdatePositionX()
    {
        PositionX = xInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionY()
    {
        PositionY = yInputField.Value;

        UpdateTile();

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

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
