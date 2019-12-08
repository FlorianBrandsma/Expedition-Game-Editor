using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SceneObjectDataElement : SceneObjectCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public SceneObjectDataElement() : base() { }

    public string objectGraphicPath;
    
    public string objectGraphicName;
    public string objectGraphicIconPath;

    public float height;
    public float width;
    public float depth;
    
    public Vector2 startPosition;

    public string originalObjectGraphicName;
    public string originalObjectGraphicIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void UpdateSearch()
    {
        base.UpdateSearch();
        
        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIconPath = objectGraphicIconPath;
    }

    public override void SetOriginalValues()
    {
        if (!Changed) return;

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

    public IDataElement Copy()
    {
        var dataElement = new SceneObjectDataElement();

        CopyGeneralData(dataElement);

        return dataElement;
    }
}
