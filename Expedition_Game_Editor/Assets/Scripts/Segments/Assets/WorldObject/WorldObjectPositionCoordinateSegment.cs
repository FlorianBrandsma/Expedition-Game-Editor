﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectPositionCoordinateSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber xInputField, yInputField, zInputField;
    public ExToggle bindToTile;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.PositionX = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.PositionX = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultPositionX = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.PositionY = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.PositionY = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultPositionY = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.PositionZ = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.PositionZ = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultPositionZ = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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

                    var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.TerrainId = terrainId;
                        interactionData.TerrainTileId = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.TerrainId = terrainId;
                        worldObjectData.TerrainTileId = value;
                    });

                    break;

                case Enums.DataType.Phase: break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    public int Time
    {
        set
        {
            switch(DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultTime = value;
                    });

                    break;
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
        if (bindToTile == null) return;

        if(bindToTile.Toggle.isOn)
        {
            var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).GeneralData.Id;

            terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);
            TerrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionZ);

        } else {
            terrainId = 0;
            TerrainTileId = 0;
        }

        DataEditor.UpdateEditor();
    }

    public void UpdateTime()
    {
        Time = TimeManager.instance.ActiveTime;

        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;
        
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.WorldObject:    InitializeWorldObjectData();    break;
            case Enums.DataType.Phase:          InitializePhaseData();          break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionElementData)DataEditor.Data.elementData;

        positionX = interactionData.PositionX;
        positionY = interactionData.PositionY;
        positionZ = interactionData.PositionZ;

        terrainTileId = interactionData.TerrainTileId;
    }

    private void InitializeWorldObjectData()
    {
        var worldObjectData = (WorldObjectElementData)DataEditor.Data.elementData;
        
        positionX = worldObjectData.PositionX;
        positionY = worldObjectData.PositionY;
        positionZ = worldObjectData.PositionZ;

        terrainTileId = worldObjectData.TerrainTileId;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseElementData)DataEditor.Data.elementData;

        positionX = phaseData.DefaultPositionX;
        positionY = phaseData.DefaultPositionY;
        positionZ = phaseData.DefaultPositionZ;

        TimeManager.instance.ActiveTime = phaseData.DefaultTime;

        terrainTileId = phaseData.terrainTileId;
    }

    private void SetSearchParameters() { }

    public void InitializeSegment()
    {
        var regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.elementData;

        var regionSize = new Vector2(regionData.RegionSize * regionData.TerrainSize * regionData.tileSize,
                                     regionData.RegionSize * regionData.TerrainSize * regionData.tileSize);

        xInputField.max = regionSize.x;
        yInputField.max = regionSize.y;

        UpdateTime();
    }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;

        if(bindToTile != null)
            bindToTile.Toggle.isOn = terrainTileId != 0;

        //Objects that are bound to tiles only load when the tile loads
        //bindToTile.EnableElement(DataEditor.Data.elementData.DataType == Enums.DataType.WorldObject);

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}