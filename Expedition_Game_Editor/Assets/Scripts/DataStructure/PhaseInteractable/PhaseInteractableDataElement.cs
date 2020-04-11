using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PhaseInteractableDataElement : PhaseInteractableCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PhaseInteractableDataElement() : base()
    {
        DataType = Enums.DataType.PhaseInteractable;
    }

    public Enums.ElementStatus elementStatus;

    public string interactableName;
    public string objectGraphicIcon;

    public string originalInteractableName;
    public string originalObjectGraphicIcon;

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
        originalObjectGraphicIcon = objectGraphicIcon;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        interactableName = originalInteractableName;
        objectGraphicIcon = originalObjectGraphicIcon;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new PhaseInteractableDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var phaseInteractableDataSource = (PhaseInteractableDataElement)dataSource;

        elementStatus = phaseInteractableDataSource.elementStatus;

        interactableName = phaseInteractableDataSource.interactableName;
        objectGraphicIcon = phaseInteractableDataSource.objectGraphicIcon;

        SetOriginalValues();
    }
}