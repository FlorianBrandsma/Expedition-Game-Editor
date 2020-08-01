public class PhaseElementData : PhaseCore, IElementData
{
    public DataElement DataElement { get; set; }

    public PhaseElementData() : base()
    {
        DataType = Enums.DataType.Phase;
    }

    public int terrainTileId;

    public int partyMemberId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string objectGraphicIconPath;
    
    public float height;
    public float width;
    public float depth;

    public float scaleMultiplier;

    public string interactableName;
    public string locationName;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new PhaseElementData();
        
        elementData.terrainTileId = terrainTileId;

        elementData.partyMemberId = partyMemberId;

        elementData.objectGraphicId = objectGraphicId;
        elementData.objectGraphicPath = objectGraphicPath;

        elementData.objectGraphicIconPath = objectGraphicIconPath;

        elementData.height = height;
        elementData.width = width;
        elementData.depth = depth;

        elementData.scaleMultiplier = scaleMultiplier;

        elementData.interactableName = interactableName;
        elementData.locationName = locationName;

        CloneCore(elementData);
        
        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var phaseDataSource = (PhaseElementData)dataSource;

        terrainTileId = phaseDataSource.terrainTileId;

        partyMemberId = phaseDataSource.partyMemberId;

        objectGraphicId = phaseDataSource.objectGraphicId;
        objectGraphicPath = phaseDataSource.objectGraphicPath;

        objectGraphicIconPath = phaseDataSource.objectGraphicIconPath;

        height = phaseDataSource.height;
        width = phaseDataSource.width;
        depth = phaseDataSource.depth;

        scaleMultiplier = phaseDataSource.scaleMultiplier;

        interactableName = phaseDataSource.interactableName;
        locationName = phaseDataSource.locationName;

        SetOriginalValues();
    }
}
