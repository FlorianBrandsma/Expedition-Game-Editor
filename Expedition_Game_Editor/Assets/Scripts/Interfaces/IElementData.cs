using UnityEngine;

public interface IElementData
{
    DataElement DataElement                 { get; set; }
    Enums.SelectionStatus SelectionStatus   { get; set; }
    bool UniqueSelection                    { get; set; }

    Enums.ExecuteType ExecuteType           { get; set; }

    Enums.DataType DataType                 { get; }

    string DebugName    { get; }

    int Id              { get; set; }

    void Add(DataRequest dataRequest);
    void Update(DataRequest dataRequest);
    void Remove(DataRequest dataRequest);

    void SetOriginalValues();
    
    bool Changed { get; }
    void ClearChanges();
    IElementData Clone();
}
