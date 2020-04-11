using UnityEngine;
using System.Collections;

public class ItemDataElement : ItemCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ItemDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new ItemDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var itemDataSource = (ItemDataElement)dataSource;

        objectGraphicPath = itemDataSource.objectGraphicPath;
        objectGraphicIconPath = itemDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
