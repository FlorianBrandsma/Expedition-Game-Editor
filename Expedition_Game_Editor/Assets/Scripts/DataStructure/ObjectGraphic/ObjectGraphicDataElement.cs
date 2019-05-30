using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectGraphicDataElement : ObjectGraphicCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ObjectGraphicDataElement() : base() { }

    public bool Changed { get { return changed; } }
}
