public class WorldObjectElementData : WorldObjectCore, IElementData
{
    public DataElement DataElement { get; set; }

    public WorldObjectElementData() : base()
    {
        DataType = Enums.DataType.WorldObject;
    }

    public string objectGraphicPath;

    public string objectGraphicName;
    public string objectGraphicIconPath;

    public float height;
    public float width;
    public float depth;

    public string originalObjectGraphicName;
    public string originalObjectGraphicIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void UpdateSearch()
    {
        base.UpdateSearch();
        
        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIconPath = objectGraphicIconPath;
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicName = originalObjectGraphicName;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new WorldObjectElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var worldObjectDataSource = (WorldObjectElementData)dataSource;
        
        objectGraphicPath = worldObjectDataSource.objectGraphicPath;

        objectGraphicName = worldObjectDataSource.objectGraphicName;
        objectGraphicIconPath = worldObjectDataSource.objectGraphicIconPath;

        height = worldObjectDataSource.height;
        width = worldObjectDataSource.width;
        depth = worldObjectDataSource.depth;
        
        SetOriginalValues();
    }
}
