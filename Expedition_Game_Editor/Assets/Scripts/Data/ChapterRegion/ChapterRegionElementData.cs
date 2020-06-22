public class ChapterRegionElementData : ChapterRegionCore, IElementData
{
    public DataElement DataElement { get; set; }

    public ChapterRegionElementData() : base()
    {
        DataType = Enums.DataType.ChapterRegion;
    }

    public string name;
    public string tileIconPath;

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
        var elementData = new ChapterRegionElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var chapterRegionDataSource = (ChapterRegionElementData)dataSource;

        name = chapterRegionDataSource.name;
        tileIconPath = chapterRegionDataSource.tileIconPath;

        SetOriginalValues();
    }
}