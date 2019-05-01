using UnityEngine;
using System.Collections.Generic;

public interface IList
{
    SelectionElement GetElement(int index);
    void SetElementSize();
    Vector2 GetListSize(int elementCount, bool exact);
    Vector2 ElementSize { get; set; }
}
