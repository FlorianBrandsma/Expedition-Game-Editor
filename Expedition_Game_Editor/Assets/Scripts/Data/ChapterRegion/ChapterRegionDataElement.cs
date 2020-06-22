using UnityEngine;
using System.Collections.Generic;

public class ChapterRegionDataElement : ChapterRegionCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public ChapterRegionDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new ChapterRegionDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var chapterRegionDataSource = (ChapterRegionDataElement)dataSource;

        name = chapterRegionDataSource.name;
        tileIconPath = chapterRegionDataSource.tileIconPath;

        SetOriginalValues();
    }
}