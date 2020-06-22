public class InteractableElementData : InteractableCore, IElementData
{
    public DataElement DataElement { get; set; }

    public InteractableElementData() : base()
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

    public IElementData Clone()
    {
        var elementData = new InteractableElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var interactableDataSource = (InteractableElementData)dataSource;

        objectGraphicPath = interactableDataSource.objectGraphicPath;
        objectGraphicIconPath = interactableDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
