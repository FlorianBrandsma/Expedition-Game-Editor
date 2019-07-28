using UnityEngine;
using System.Collections;

public interface IDataElement
{
    SelectionElement SelectionElement { get; set; }
    int Id { get; }
    void Update();
    void UpdateSearch();
    bool Changed { get; }
    void ClearChanges();
}
