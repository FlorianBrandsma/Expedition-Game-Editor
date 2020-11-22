using UnityEngine;

public interface IElementData
{
    DataElement DataElement                 { get; set; }
    Enums.SelectionStatus SelectionStatus   { get; set; }
    bool UniqueSelection                    { get; set; }

    Enums.DataType DataType { get; }

    string DebugName { get; }

    int Id { get; set; }

    void Update();
    void UpdateSearch();
    void SetOriginalValues();

    bool Changed { get; }
    void ClearChanges();
    IElementData Clone();
}
