using UnityEngine;
using System.Linq;

public class RegionCore : GeneralData
{
    private int chapterRegionId;
    private int phaseId;
    private int tileSetId;
    private string name;
    private int regionSize;
    private int terrainSize;

    public int originalChapterRegionId;
    public int originalPhaseId;
    public int originalTileSetId;
    public string originalName;
    public int originalRegionSize;
    public int originalTerrainSize;

    private bool changedChapterRegionId;
    private bool changedPhaseId;
    public bool changedTileSetId;
    public bool changedName;
    private bool changedRegionSize;
    private bool changedTerrainSize;
    
    public bool Changed
    {
        get
        {
            return changedChapterRegionId || changedPhaseId || changedTileSetId || changedName || changedRegionSize || changedTerrainSize;
        }
    }

    #region Properties

    public int ChapterRegionId
    {
        get { return chapterRegionId; }
        set
        {
            if (value == chapterRegionId) return;

            changedChapterRegionId = (value != originalChapterRegionId);

            chapterRegionId = value;
        }
    }

    public int PhaseId
    {
        get { return phaseId; }
        set
        {
            if (value == phaseId) return;

            changedPhaseId = (value != originalPhaseId);

            phaseId = value;
        }
    }

    public int TileSetId
    {
        get { return tileSetId; }
        set
        {
            if (value == tileSetId) return;

            changedTileSetId = (value != originalTileSetId);

            tileSetId = value;
        }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);

            name = value;
        }
    }

    public int RegionSize
    {
        get { return regionSize; }
        set
        {
            if (value == regionSize) return;

            changedRegionSize = (value != originalRegionSize);

            regionSize = value;
        }
    }

    public int TerrainSize
    {
        get { return terrainSize; }
        set
        {
            if (value == terrainSize) return;

            changedTerrainSize = (value != originalTerrainSize);

            terrainSize = value;
        }
    }

    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update()
    {
        var regionData = Fixtures.regionList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedTileSetId)
            regionData.tileSetId = tileSetId;

        if (changedName)
            regionData.name = name;

        if (changedRegionSize)
            regionData.regionSize = regionSize;

        if (changedTerrainSize)
            regionData.terrainSize = terrainSize;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var regionData = Fixtures.regionList.Where(x => x.Id == Id).FirstOrDefault();

        regionData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalChapterRegionId = chapterRegionId;
        originalPhaseId = phaseId;
        originalTileSetId = tileSetId;
        originalName = name;
        originalRegionSize = regionSize;
        originalTerrainSize = terrainSize;
    }

    public void GetOriginalValues()
    {
        chapterRegionId = originalChapterRegionId;
        phaseId = originalPhaseId;
        tileSetId = originalTileSetId;
        name = originalName;
        regionSize = originalRegionSize;
        terrainSize = originalTerrainSize;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedChapterRegionId = false;
        changedPhaseId = false;
        changedTileSetId = false;
        changedName = false;
        changedRegionSize = false;
        changedTerrainSize = false;
    }

    public void Delete() { }

    #endregion
}
