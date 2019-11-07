using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PhaseInteractableDataElement : PhaseInteractableCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PhaseInteractableDataElement() : base() { }

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
        if (!Changed) return;

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
        base.ClearChanges();

        GetOriginalValues();
    }
}