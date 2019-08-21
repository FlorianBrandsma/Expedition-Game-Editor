using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class RegionDataElement : RegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public RegionDataElement() : base() { }

    public SceneDataElement sceneDataElement;

    public Enums.RegionType type;
}
