using UnityEngine;
using System.Collections.Generic;

public interface IList
{
    List<SelectionElement> ElementList { get; set; }
    Vector2 GetListSize(int elementCount, bool exact);
    Vector2 ElementSize { get; }
    Vector2 GetElementPosition(int index);
}
