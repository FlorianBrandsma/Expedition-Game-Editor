using UnityEngine;
using System.Linq;

public class InteractionDestinationDestinationCoordinateSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    private float positionX, positionY, positionZ;
    private int terrainTileId;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float PositionX
    {
        get { return positionX; }
        set
        {
            positionX = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.PositionX = value;
            });
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            positionY = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.PositionY = value;
            });
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            positionZ = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.PositionZ = value;
            });
        }
    }

    public int TerrainTileId
    {
        get { return terrainTileId; }
        set
        {
            terrainTileId = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.TerrainTileId = value;
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
        var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData.Id;

        var terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);
        TerrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionZ);

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

        var interactionDestinationData = (InteractionDestinationElementData)DataEditor.ElementData;

        positionX = interactionDestinationData.PositionX;
        positionY = interactionDestinationData.PositionY;
        positionZ = interactionDestinationData.PositionZ;
        
        terrainTileId = interactionDestinationData.TerrainTileId;
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

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}