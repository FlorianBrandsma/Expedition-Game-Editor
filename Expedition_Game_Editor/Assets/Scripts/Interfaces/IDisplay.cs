﻿using UnityEngine;

public interface IDisplay
{
    void ClearDisplay();
    void CloseDisplay();
    
    Enums.DisplayType DisplayType { get; }

    IProperties Properties { get; }
    IDataController DataController { get; set; }

    SelectionManager.Property SelectionProperty { get; }
    SelectionManager.Type SelectionType { get; }
}
