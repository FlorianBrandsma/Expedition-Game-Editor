using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class IconDataElement : IconCore, IDataElement
{
    public IconDataElement() : base() { }

    public bool Changed { get { return changed; } }
}