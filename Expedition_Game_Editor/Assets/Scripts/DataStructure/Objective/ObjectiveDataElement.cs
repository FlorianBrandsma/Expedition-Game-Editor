using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectiveDataElement : ObjectiveCore, IDataElement
{
    public ObjectiveDataElement() : base() { }

    public bool Changed { get { return changed; } }
}
