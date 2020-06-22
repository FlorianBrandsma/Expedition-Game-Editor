﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataManager
{
    IDataController DataController { get; set; }
    List<IElementData> GetData(SearchProperties searchProperties);
}
