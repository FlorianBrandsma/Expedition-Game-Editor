using UnityEngine;
using System.Collections;

public interface IDataElement
{
    void Update();
    bool Changed { get; }
    void ClearChanges();
}
