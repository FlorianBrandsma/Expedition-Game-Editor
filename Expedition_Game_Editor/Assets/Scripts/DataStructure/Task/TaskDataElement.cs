using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TaskDataElement : TaskCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TaskDataElement() : base() { }

    public int regionId;

    public string regionName;
    public string objectGraphicIconPath;
}
