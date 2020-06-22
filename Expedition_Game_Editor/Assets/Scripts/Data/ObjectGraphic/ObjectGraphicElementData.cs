public class ObjectGraphicElementData : ObjectGraphicCore, IElementData
{
    public DataElement DataElement { get; set; }

    public ObjectGraphicElementData() : base()
    {
        DataType = Enums.DataType.ObjectGraphic;
    }

    public int category;
    public string iconPath;

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
        var elementData = new ObjectGraphicElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var objectGraphicDataSource = (ObjectGraphicElementData)dataSource;

        category = objectGraphicDataSource.category;
        iconPath = objectGraphicDataSource.iconPath;

        SetOriginalValues();
    }
}
