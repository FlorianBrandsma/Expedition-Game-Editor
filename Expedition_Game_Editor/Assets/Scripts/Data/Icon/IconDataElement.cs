using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class IconDataElement : IconCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public IconDataElement() : base()
    {
        DataType = Enums.DataType.Icon;
    }

    public string baseIconPath;

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
        var dataElement = new IconDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var iconDataSource = (IconDataElement)dataSource;

        baseIconPath = iconDataSource.baseIconPath;

        SetOriginalValues();
    }
}