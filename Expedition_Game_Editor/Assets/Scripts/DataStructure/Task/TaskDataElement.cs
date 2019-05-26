﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TaskDataElement : TaskCore, IDataElement
{
    public TaskDataElement() : base() { }

    public string objectGraphicIcon;
    public string regionName;

    public bool Changed { get { return changed; } }
}
