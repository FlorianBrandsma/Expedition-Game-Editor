using UnityEngine;
using System.Collections;

public interface IDataElement
{
    SelectionElement SelectionElement { get; set; }
    int Id { get; set; }
    int Index { get; set; }
    void Update();
    void UpdateSearch();
    void UpdateIndex();
    bool Changed { get; }
    void ClearChanges();
}
