public class ItemElementData : ItemCore, IElementData
{
    public DataElement DataElement { get; set; }

    public ItemElementData() : base()
    {
        DataType = Enums.DataType.Item;
    }

    public string objectGraphicPath;
    public string objectGraphicIconPath;

    public string originalObjectGraphicPath;
    public string originalObjectGraphicIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalObjectGraphicPath = objectGraphicPath;
        originalObjectGraphicIconPath = objectGraphicIconPath;
        
        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicPath = originalObjectGraphicPath;
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
        var elementData = new ItemElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var itemDataSource = (ItemElementData)dataSource;

        objectGraphicPath = itemDataSource.objectGraphicPath;
        objectGraphicIconPath = itemDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
