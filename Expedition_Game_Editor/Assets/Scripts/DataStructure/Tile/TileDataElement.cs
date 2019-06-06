using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileDataElement : TileCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TileDataElement() : base() { }

    public string icon;
    public string originalIcon;
}
