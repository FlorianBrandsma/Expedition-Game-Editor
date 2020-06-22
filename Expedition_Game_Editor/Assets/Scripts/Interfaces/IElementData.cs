using UnityEngine;
using System.Collections;

public interface IElementData
{
    DataElement DataElement { get; set; }
    Enums.SelectionStatus SelectionStatus { get; set; }
    Enums.DataType DataType { get; set; }
    int Id { get; set; }
    int Index { get; set; }
    void Update();
    void UpdateSearch();
    void UpdateIndex();
    void SetOriginalValues();
    bool Changed { get; }
    void ClearChanges();
    IElementData Clone();
    void Copy(IElementData dataSource);
}
