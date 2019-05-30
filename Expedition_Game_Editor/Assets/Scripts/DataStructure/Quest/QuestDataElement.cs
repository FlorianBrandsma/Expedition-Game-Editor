using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestDataElement : QuestCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public QuestDataElement() : base() { }

    public bool Changed { get { return changed; } }
}