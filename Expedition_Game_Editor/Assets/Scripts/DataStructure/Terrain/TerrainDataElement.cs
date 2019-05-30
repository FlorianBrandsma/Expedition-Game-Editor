using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainDataElement : TerrainCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TerrainDataElement() : base() { }

    public string icon;

    public bool Changed { get { return changed; } }
}