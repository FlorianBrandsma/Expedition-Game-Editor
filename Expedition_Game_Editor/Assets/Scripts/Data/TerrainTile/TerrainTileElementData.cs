public class TerrainTileElementData : TerrainTileCore, IElementData
{
    public DataElement DataElement { get; set; }

    public TerrainTileElementData() : base()
    {
        DataType = Enums.DataType.TerrainTile;
    }

    public bool active;

    public GridElement gridElement;

    public string iconPath;

    public string originalIconPath;

    public override void Update()
    {
        if (!Changed) return;
        
        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalIconPath = iconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        iconPath = originalIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new TerrainTileElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var terrainTileDataSource = (TerrainTileElementData)dataSource;

        iconPath = terrainTileDataSource.iconPath;

        SetOriginalValues();
    }
}
