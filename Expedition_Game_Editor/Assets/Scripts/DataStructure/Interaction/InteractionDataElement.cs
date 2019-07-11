using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InteractionDataElement : InteractionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public InteractionDataElement() : base() { }

    public string regionName;
    public string objectGraphicIconPath;
}
