using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PhaseRegionDataElement : PhaseRegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PhaseRegionDataElement() : base() { }

    public bool Changed { get { return changed; } }
}