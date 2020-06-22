using UnityEngine;
using System.Collections.Generic;

public class ObjectGraphicDataElement : ObjectGraphicCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public ObjectGraphicDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new ObjectGraphicDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var objectGraphicDataSource = (ObjectGraphicDataElement)dataSource;

        category = objectGraphicDataSource.category;
        iconPath = objectGraphicDataSource.iconPath;

        SetOriginalValues();
    }
}
