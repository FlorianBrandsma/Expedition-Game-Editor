using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RegionDataElement : RegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public RegionDataElement() : base() { }

    public Enums.RegionType type;
}
