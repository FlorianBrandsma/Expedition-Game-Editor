using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SceneObjectDataElement : SceneObjectCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public SceneObjectDataElement() : base() { }

    public string objectGraphicName;
    public string objectGraphicIconPath;

    public string originalObjectGraphicName;
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

        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicName = originalObjectGraphicName;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
