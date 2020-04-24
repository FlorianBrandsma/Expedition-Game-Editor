﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldElementTransformPositionCoordinateSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public EditorInputNumber xInputField, yInputField, zInputField;
    public EditorToggle bindToTile;
    #endregion

    #region Data Variables
    private float positionX, positionY, positionZ;
    private int terrainId;
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

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.PositionX = value;
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

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.PositionY = value;
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

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.PositionZ = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public int TerrainTileId
    {
        get { return terrainTileId; }
        set
        {
            terrainTileId = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionDataList = DataEditor.DataList.Cast<InteractionDataElement>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.TerrainId = terrainId;
                        interactionData.TerrainTileId = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectDataElement>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.TerrainId = terrainId;
                        worldObjectData.TerrainTileId = value;
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
        if(bindToTile.Toggle.isOn)
        {
            var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).GeneralData.Id;

            terrainId = Fixtures.GetTerrain(regionId, positionX, positionY);
            TerrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionY);

        } else {
            terrainId = 0;
            TerrainTileId = 0;
        }

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

    public void InitializeSegment()
    {
        var regionData = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;
        var worldDataElement = regionData.worldDataElement;

        var regionSize = new Vector2(worldDataElement.regionSize * worldDataElement.terrainSize * worldDataElement.tileSize,
                                     worldDataElement.regionSize * worldDataElement.terrainSize * worldDataElement.tileSize);

        xInputField.max = regionSize.x;
        yInputField.max = regionSize.y;
    }

    public void InitializeData()
    {
        InitializeDependencies();
        
        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.WorldObject:    InitializeworldObjectData();    break;

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

    private void InitializeworldObjectData()
    {
        var worldObjectData = (WorldObjectDataElement)DataEditor.Data.dataElement;
        
        positionX = worldObjectData.PositionX;
        positionY = worldObjectData.PositionY;
        positionZ = worldObjectData.PositionZ;

        terrainTileId = worldObjectData.TerrainTileId;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;

        bindToTile.Toggle.isOn = terrainTileId != 0;

        //Binding to tile is meant to render large objects that span multiple terrains.
        //If necessary, modifications could be made to better support this feature for interacions
        bindToTile.EnableElement(DataEditor.Data.dataElement.DataType == Enums.DataType.WorldObject);

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}