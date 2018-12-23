using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IController
{
    Path path           { get; set; }

    bool loaded         { get; set; }

    ElementData data    { get; set; }

    EditorSection section   { get; set; }

    void FilterRows(List<ElementData> list);
}
