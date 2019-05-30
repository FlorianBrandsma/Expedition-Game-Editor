using UnityEngine;
using System.Collections;

public interface IDataElement
{
    SelectionElement SelectionElement { get; set; }
    void Update();
    bool Changed { get; }
    void ClearChanges();
}
