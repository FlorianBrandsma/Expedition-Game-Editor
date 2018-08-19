using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IController
{
    ElementData data { get; set; }

    EditorField field { get; set; }

    void FilterRows(List<ElementData> list);
}
