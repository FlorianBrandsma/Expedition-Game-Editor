﻿using UnityEngine;
using System.Collections.Generic;

public interface IList
{
    List<SelectionElement> ElementList { get; set; }
    void SetElementSize();
    Vector2 GetListSize(int elementCount, bool exact);
    Vector2 ElementSize { get; set; }
}
