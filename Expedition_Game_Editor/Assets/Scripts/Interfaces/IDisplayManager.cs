﻿using UnityEngine;
using System.Collections;

public interface IDisplayManager
{
    IOrganizer Organizer { get; set; }
    RectTransform RectTransform { get; }
    IDisplay Display { get; }

    void UpdateData();
    void UpdateOverlay();
    void CorrectPosition(IElementData data);
}
