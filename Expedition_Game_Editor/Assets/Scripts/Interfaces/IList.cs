using UnityEngine;
using System.Collections.Generic;

public interface IList
{
    SelectionElement GetElement(int index);
    void SetElementSize();
    Vector2 GetListSize(int element_count, bool exact);
    Vector2 element_size { get; set; }
}
