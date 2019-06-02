using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectiveDataElement : ObjectiveCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ObjectiveDataElement() : base() { }
}
