using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestElementDataElement : QuestElementCore, IDataElement
{
    public QuestElementDataElement() : base() { }

    public bool Changed { get { return changed; } }
}