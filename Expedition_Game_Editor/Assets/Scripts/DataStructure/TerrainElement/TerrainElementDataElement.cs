using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainElementDataElement : TerrainElementCore, IDataElement
{
    public TerrainElementDataElement() : base() { }

    public string name;
    public string objectGraphicIcon;

    public override void Update()
    {
        if (!changed) return;

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
        base.ClearChanges();

        GetOriginalValues();
    }

    public bool Changed { get { return changed; } }
}
