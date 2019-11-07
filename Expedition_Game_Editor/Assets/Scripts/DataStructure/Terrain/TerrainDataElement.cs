using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainDataElement : TerrainCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TerrainDataElement() : base() { }

    public string iconPath;
    public string baseTilePath;

    public int tileSetId;

    public string originalIconPath;

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

        originalIconPath = iconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        iconPath = originalIconPath;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}