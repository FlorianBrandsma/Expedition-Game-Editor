using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PartyMemberDataElement : PartyMemberCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PartyMemberDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new PartyMemberDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var partyMemberDataSource = (PartyMemberDataElement)dataSource;

        interactableName = partyMemberDataSource.interactableName;
        objectGraphicIconPath = partyMemberDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
