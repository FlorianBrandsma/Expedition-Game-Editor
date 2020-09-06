using UnityEngine;
using System.Linq;

public class WorldObjectPositionCoordinateSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;
    public ExToggle bindToTile;

    private float positionX, positionY, positionZ;
    private int terrainId;
    private int terrainTileId;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float PositionX
    {
        get { return positionX; }
        set
        {
            positionX = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.PositionX = value;
            });
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            positionY = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.PositionY = value;
            });
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            positionZ = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.PositionZ = value;
            });
        }
    }

    public int TerrainTileId
    {
        get { return terrainTileId; }
        set
        {
            terrainTileId = value;

            var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
            worldObjectDataList.ForEach(worldObjectData =>
            {
                worldObjectData.TerrainId = terrainId;
                worldObjectData.TerrainTileId = value;
            });
        }
    }

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
            var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData.Id;

            terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);
            TerrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionZ);

        } else {
            terrainId = 0;
            TerrainTileId = 0;
        }

        DataEditor.UpdateEditor();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var worldObjectData = (WorldObjectElementData)DataEditor.ElementData;

        positionX = worldObjectData.PositionX;
        positionY = worldObjectData.PositionY;
        positionZ = worldObjectData.PositionZ;

        terrainTileId = worldObjectData.TerrainTileId;
    }

    public void InitializeSegment()
    {
        var regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;

        var regionSize = new Vector2(regionData.RegionSize * regionData.TerrainSize * regionData.TileSize,
                                     regionData.RegionSize * regionData.TerrainSize * regionData.TileSize);

        xInputField.max = regionSize.x;
        yInputField.max = regionSize.y;
    }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;
        
        bindToTile.Toggle.isOn = terrainTileId != 0;
        
        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
