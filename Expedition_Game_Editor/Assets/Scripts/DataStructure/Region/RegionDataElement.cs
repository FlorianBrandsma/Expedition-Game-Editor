using UnityEngine;
using System;
using System.Collections.Generic;


[System.Serializable]
public class RegionDataElement : RegionCore, IDataElement, ICloneable
{
    public SelectionElement SelectionElement { get; set; }

    public RegionDataElement() : base() { }

    public Enums.RegionType type;

    public object Clone()
    {
        return MemberwiseClone();
    }
}
