using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class IconDataElement : IconCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public IconDataElement() : base() { }
}