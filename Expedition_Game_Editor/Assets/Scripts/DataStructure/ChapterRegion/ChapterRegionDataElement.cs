using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChapterRegionDataElement : ChapterRegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ChapterRegionDataElement() : base() { }

    public string name;

    public bool Changed { get { return changed; } }
}