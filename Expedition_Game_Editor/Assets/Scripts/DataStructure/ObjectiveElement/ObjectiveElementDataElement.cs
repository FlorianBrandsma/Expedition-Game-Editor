using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectiveElementDataElement : ObjectiveElementCore, IDataElement
{
    public ObjectiveElementDataElement() : base() { }

    public bool Changed { get { return changed; } }
}