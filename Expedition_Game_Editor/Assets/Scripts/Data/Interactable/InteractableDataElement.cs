using UnityEngine;
using System.Collections;

public class InteractableDataElement : InteractableCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public InteractableDataElement() : base()
    {
        DataType = Enums.DataType.Interactable;
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
        var dataElement = new InteractableDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var interactableDataSource = (InteractableDataElement)dataSource;

        objectGraphicPath = interactableDataSource.objectGraphicPath;
        objectGraphicIconPath = interactableDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
