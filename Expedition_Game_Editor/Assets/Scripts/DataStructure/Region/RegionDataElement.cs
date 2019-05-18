using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RegionDataElement : RegionCore, IDataElement
{
    public RegionDataElement() : base() { }

    public int type;

    public bool Changed { get { return changed; } }
}
