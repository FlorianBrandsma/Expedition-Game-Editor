using UnityEngine;
using System.Linq;

public class PhaseTransformPositionCoordinateSegment : MonoBehaviour, ISegment
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

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultPositionX = value;
            });
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            positionY = value;

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultPositionY = value;
            });
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            positionZ = value;

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultPositionZ = value;
            });
        }
    }

    public int TerrainTileId
    {
        get { return terrainTileId; }
        set
        {
            terrainTileId = value;

            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.terrainTileId = value;
            });       
        }
    }

    public int Time
    {
        set
        {
            var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
            phaseDataList.ForEach(phaseData =>
            {
                phaseData.DefaultTime = value;
            });
        }
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

        var phaseData = (PhaseElementData)DataEditor.Data.elementData;

        positionX = phaseData.DefaultPositionX;
        positionY = phaseData.DefaultPositionY;
        positionZ = phaseData.DefaultPositionZ;

        TimeManager.instance.ActiveTime = phaseData.DefaultTime;

        terrainTileId = phaseData.terrainTileId;
    }

    public void InitializeSegment()
    {
        var regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.elementData;

        var regionSize = new Vector2(regionData.RegionSize * regionData.TerrainSize * regionData.tileSize,
                                     regionData.RegionSize * regionData.TerrainSize * regionData.tileSize);

        xInputField.max = regionSize.x;
        yInputField.max = regionSize.y;

        UpdateTime();
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
        var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).GeneralData.Id;

        var terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);
        TerrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionZ);

        DataEditor.UpdateEditor();
    }

    public void UpdateTime()
    {
        Time = TimeManager.instance.ActiveTime;

        DataEditor.UpdateEditor();
    }

    private void SetSearchParameters() { }

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

