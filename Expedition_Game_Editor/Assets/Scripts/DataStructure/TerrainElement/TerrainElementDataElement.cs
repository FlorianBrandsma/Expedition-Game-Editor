using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainElementDataElement : TerrainElementCore, IDataElement
{
    public TerrainElementDataElement() : base() { }

    public string icon;
    public string name;

    public bool Changed { get { return changed; } }
}
