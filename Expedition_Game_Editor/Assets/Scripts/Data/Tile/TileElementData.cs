public class TileElementData : TileCore, IElementData
{
    public DataElement DataElement { get; set; }

    public TileElementData() : base()
    {
        DataType = Enums.DataType.Tile;
    }

    public string icon;

    public string originalIcon;

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
        var elementData = new TileElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var tileDataSource = (TileElementData)dataSource;

        icon = tileDataSource.icon;

        SetOriginalValues();
    }
}
