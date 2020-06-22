public class PartyMemberElementData : PartyMemberCore, IElementData
{
    public DataElement DataElement { get; set; }

    public PartyMemberElementData() : base()
    {
        DataType = Enums.DataType.PartyMember;
    }

    public string interactableName;
    public string objectGraphicIconPath;

    public string originalInteractableName;
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

        originalInteractableName = interactableName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        interactableName = originalInteractableName;
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
        var elementData = new PartyMemberElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var partyMemberDataSource = (PartyMemberElementData)dataSource;

        interactableName = partyMemberDataSource.interactableName;
        objectGraphicIconPath = partyMemberDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
